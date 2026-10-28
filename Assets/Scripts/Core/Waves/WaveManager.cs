using FckAI.Enemies;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.Core.Waves
{
    public partial class WaveManager : MonoBehaviour
    {
        [AutoStaticsCleanup]
        public static WaveManager s_Instance;
        
        private WaveData m_currentWave;

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
            m_currentWave = GameManager.s_Instance.m_GameData.m_StartWave;
        }

        public void StartWave()
        {
            GameManager.AddTokens(m_currentWave.m_ExtraStartTokens);
            StartCoroutine(EnemyManager.s_Instance.SpawnNewWave(m_currentWave));
        }

        public void EndWave()
        {
            GoToNextWave();
        }

        private void GoToNextWave()
        {
            if (m_currentWave.m_NextWave == null)
            {
                Debug.Log("All waves cleared");
                return;
            }
            m_currentWave = m_currentWave.m_NextWave;
            StartWave();
        }
    }
}