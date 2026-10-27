using FckAI.Core.Waves;
using FckAI.UI.Towers;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.Core
{
    [AutoStaticsCleanup]
    public partial class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private GameData m_gameData;
        public GameData m_GameData => m_gameData;

        [SerializeField] private Camera m_camera;
        public Camera m_Camera => m_camera;

        public static int s_Tokens = 0;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            FindAnyObjectByType<TowerSelectUI>().Initialise();
            FindAnyObjectByType<TowerUpgradeUI>().Initialise();
            FindAnyObjectByType<WaveManager>().Initialise();
            
            WaveManager.s_Instance.StartWave();
        }
    }
}