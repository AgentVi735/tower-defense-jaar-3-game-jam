using System.Collections.Generic;
using FckAI.Core;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.Enemies
{
    public partial class EnemyManager : MonoBehaviour
    {
        [AutoStaticsCleanup]
        public static EnemyManager s_Instance;

        private Dictionary<EnemyData, EnemyController[]> m_enemies;

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
            
            CreateMaxEnemies();
        }

        private void CreateMaxEnemies()
        {
            EnemyData[] enemies = GameManager.Instance.m_GameData.m_EnemyOrder;

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
        }
    }
}