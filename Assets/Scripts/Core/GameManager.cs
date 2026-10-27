using FckAI.Core.Waves;
using FckAI.Enemies;
using FckAI.UI.Towers;
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

        public static int s_Tokens = 0;

        private void Awake()
        {
            if (s_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            FindAnyObjectByType<TowerSelectUI>().Initialise();
            FindAnyObjectByType<TowerUpgradeUI>().Initialise();
            FindAnyObjectByType<WaveManager>().Initialise();
            FindAnyObjectByType<EnemyManager>().Initialise();
            
            WaveManager.s_Instance.StartWave();
        }
    }
}