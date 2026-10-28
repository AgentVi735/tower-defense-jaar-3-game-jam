using FckAI.Core.Waves;
using FckAI.Enemies;
using FckAI.UI.Towers;
using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.Core
{
    [AutoStaticsCleanup]
    public partial class GameManager : MonoBehaviour
    {
        public static GameManager s_Instance;
        
        [SerializeField] private GameData m_gameData;
        public GameData m_GameData => m_gameData;

        [SerializeField] private Camera m_camera;
        public Camera m_Camera => m_camera;

        public static int s_Tokens { get; private set; }

        [SerializeField] private TMP_Text m_tokensText;

        private void Awake()
        {
            if (s_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            s_Tokens = 0;
            FindAnyObjectByType<BaseManager>().Initialise();
            FindAnyObjectByType<TowerSelectUI>().Initialise();
            FindAnyObjectByType<TowerUpgradeUI>().Initialise();
            FindAnyObjectByType<WaveManager>().Initialise();
            FindAnyObjectByType<EnemyManager>().Initialise();
            
            WaveManager.s_Instance.StartWave();
        }

        private void OnDestroy()
        {
            if (s_Instance == this)
                s_Instance = null;
        }

        public static void AddTokens(int amount)
        {
            s_Tokens += amount;
            s_Instance.m_tokensText.text = s_Tokens.ToString();
            if (TowerSelectUI.s_Instance.m_IsActive)
                TowerSelectUI.s_Instance.RefreshAvailability();
            else if (TowerUpgradeUI.s_Instance.m_IsActive)
                TowerUpgradeUI.s_Instance.RefreshAvailability();
        }
    }
}