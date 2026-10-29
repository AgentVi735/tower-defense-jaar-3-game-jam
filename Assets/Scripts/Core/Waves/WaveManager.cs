using System.Collections;
using FckAI.Enemies;
using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.Core.Waves
{
    public partial class WaveManager : MonoBehaviour
    {
        [AutoStaticsCleanup]
        public static WaveManager s_Instance;

        [SerializeField] private float m_timeBetweenWaves;
        private WaitForSeconds m_waitTimeBetweenWaves;
        
        private WaveData m_currentWave;
        private int m_waveCount;

        [SerializeField] private TMP_Text m_waveText;

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
            m_waveCount = 1;
            m_waveText.text = $"Wave {m_waveCount}";
            m_waitTimeBetweenWaves = new WaitForSeconds(m_timeBetweenWaves);
        }

        public void StartWave()
        {
            StartCoroutine(StartWaveCoroutine());
        }

        private IEnumerator StartWaveCoroutine()
        {
            GameManager.AddTokens(m_currentWave.m_ExtraStartTokens);
            yield return m_waitTimeBetweenWaves;
            StartCoroutine(EnemyManager.s_Instance.SpawnNewWave(m_currentWave));
        }

        public void EndWave()
        {
            StartCoroutine(GoToNextWave());
        }

        private IEnumerator GoToNextWave()
        {
            if (m_currentWave.m_NextWave == null)
            {
                Debug.Log("All waves cleared");
                GameManager.s_Instance.StopGame();
                yield break;
            }
            m_currentWave = m_currentWave.m_NextWave;
            m_waveCount++;
            m_waveText.text = $"Wave {m_waveCount}";
            StartWave();
        }
    }
}