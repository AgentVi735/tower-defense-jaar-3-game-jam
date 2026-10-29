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
        public List<EnemyController> m_ActiveEnemies { get; private set; }

        [SerializeField] private EnemyWaypoint m_startingPoint;
        public EnemyWaypoint m_StartingPoint => m_startingPoint;

        [SerializeField] private float m_spawnDelay;
        private WaitForSeconds m_waitSpawnDelay;

        private bool m_hasSpawnedAllEnemies;

        private void Awake()
        {
            if (s_Instance == null) return;
            if (s_Instance == this) return;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (s_Instance == this)
                s_Instance = null;
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

            m_ActiveEnemies = new List<EnemyController>();
        }

        public IEnumerator SpawnNewWave(WaveData waveData)
        {
            m_hasSpawnedAllEnemies = false;
            foreach (EnemyData enemyData in GameManager.s_Instance.m_GameData.m_EnemyOrder)
            {
                EnemyModifierData modifier = waveData.GetEnemyModifier(enemyData);
                bool hasModifier = modifier != null;

                bool isInWave = waveData.m_EnemyCount.ContainsKey(enemyData);
                int count = -1;
                if (isInWave)
                {
                    bool couldGetValue = waveData.m_EnemyCount.TryGetValue(enemyData, out count);
                    if (!couldGetValue)
                    {
                        Debug.LogError($"Unable to get enemy count of {enemyData.m_EnemyName}");
                        continue;
                    }
                }
                
                bool couldGetEnemies = m_enemies.TryGetValue(enemyData, out EnemyController[] enemies);
                if (!couldGetEnemies)
                {
                    Debug.LogError($"Unable to get enemyControllers of {enemyData.m_EnemyName}");
                    continue;
                }

                if (hasModifier)
                    foreach (EnemyController enemy in enemies)
                        enemy.SetModifier(modifier);

                if (enemies.Length < count)
                {
                    Debug.LogError($"Enemy {enemyData.m_EnemyName} has too little max enemies. Currently max is {enemies.Length} and the needed amount is {count}");
                    continue;
                }
                
                for (int i = 0; i < count; i++)
                {
                    EnemyController controller = enemies[i];
                    controller.EnableEnemy();
                    m_ActiveEnemies.Add(controller);
                    yield return m_waitSpawnDelay;
                }
            }

            m_hasSpawnedAllEnemies = true;
            
            if (m_ActiveEnemies.Count == 0)
                WaveManager.s_Instance.EndWave();
        }

        private void Update()
        {
            if (m_ActiveEnemies.Count == 0)
                return;
            
            foreach (EnemyController enemy in m_ActiveEnemies.ToArray())
            {
                enemy.Move();
            }
        }
        
        public void DisableEnemy(EnemyController controller, bool shouldDoDamage)
        {
            controller.gameObject.SetActive(false);
            m_ActiveEnemies.Remove(controller);
            if (shouldDoDamage)
                BaseManager.s_Instance.AddHealth(-controller.GetDamage());
            else
                GameManager.AddTokens(controller.m_Data.m_Reward);
            
            if (m_hasSpawnedAllEnemies && m_ActiveEnemies.Count == 0)
                WaveManager.s_Instance.EndWave();
        }
    }
}