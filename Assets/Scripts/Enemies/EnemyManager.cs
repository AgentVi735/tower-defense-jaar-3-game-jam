using System.Collections;
using System.Collections.Generic;
using FckAI.Core;
using FckAI.Core.Waves;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.Enemies
{
    public partial class EnemyManager : MonoBehaviour
    {
        [AutoStaticsCleanup]
        public static EnemyManager s_Instance;

        private Dictionary<EnemyData, EnemyController[]> m_enemies;
        private List<EnemyController> m_activeEnemies;

        [SerializeField] private EnemyWaypoint m_startingPoint;
        public EnemyWaypoint m_StartingPoint => m_startingPoint;

        [SerializeField] private float m_spawnDelay;
        private WaitForSeconds m_waitSpawnDelay;

        private void Awake()
        {
            if (s_Instance == null) return;
            if (s_Instance == this) return;
            Destroy(gameObject);
        }

        public void Initialise()
        {
            if (s_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;

            m_waitSpawnDelay = new WaitForSeconds(m_spawnDelay);
            
            CreateMaxEnemies();
        }

        private void CreateMaxEnemies()
        {
            EnemyData[] enemies = GameManager.s_Instance.m_GameData.m_EnemyOrder;

            m_enemies = new Dictionary<EnemyData, EnemyController[]>();
            
            foreach (EnemyData enemyData in enemies)
            {
                List<EnemyController> controllers = new();

                for (int i = 0; i < enemyData.m_MaxEnemyCount; i++)
                {
                    EnemyController enemy = Instantiate(enemyData.m_EnemyPrefab, transform);
                    enemy.Initialise(enemyData);
                    controllers.Add(enemy);
                }

                m_enemies.TryAdd(enemyData, controllers.ToArray());
            }

            m_activeEnemies = new List<EnemyController>();
        }

        public IEnumerator SpawnNewWave(WaveData waveData)
        {
            foreach (EnemyData enemyData in GameManager.s_Instance.m_GameData.m_EnemyOrder)
            {
                if (!waveData.m_EnemyCount.ContainsKey(enemyData)) continue;
                bool couldGetValue = waveData.m_EnemyCount.TryGetValue(enemyData, out int count);
                if (!couldGetValue)
                {
                    Debug.LogError($"Unable to get enemy count of {enemyData.m_EnemyName}");
                    continue;
                }

                bool couldGetEnemies = m_enemies.TryGetValue(enemyData, out EnemyController[] enemies);
                if (!couldGetEnemies)
                {
                    Debug.LogError($"Unable to get enemyControllers of {enemyData.m_EnemyName}");
                    continue;
                }

                if (enemies.Length < count)
                {
                    Debug.LogError($"Enemy {enemyData.m_EnemyName} has too little max enemies. Currently max is {enemies.Length} and the needed amount is {count}");
                    continue;
                }
                
                for (int i = 0; i < count; i++)
                {
                    EnemyController controller = enemies[i];
                    controller.EnableEnemy();
                    m_activeEnemies.Add(controller);
                    yield return m_waitSpawnDelay;
                }
            }
        }

        private void Update()
        {
            if (m_activeEnemies.Count == 0)
                return;
            
            foreach (EnemyController enemy in m_activeEnemies.ToArray())
            {
                enemy.Move();
            }
        }
        
        public void DisableEnemy(EnemyController controller)
        {
            controller.gameObject.SetActive(false);
            m_activeEnemies.Remove(controller);
        }
    }
}