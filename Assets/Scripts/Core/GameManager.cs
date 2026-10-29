using System.Collections;
using FckAI.Core.Waves;
using FckAI.Enemies;
using FckAI.UI.Towers;
using TMPro;
using Unity.Scripting.LifecycleManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        [SerializeField] private CanvasGroup m_mainMenuCanvasGroup;
        [SerializeField] private float m_canvasGroupFadeTime;

        [SerializeField] private float m_timeBeforeStart;
        private WaitForSeconds m_waitTimeBeforeStart;
        
#if UNITY_EDITOR
        [SerializeField] private SceneAsset m_mainMenuScene;
        private void OnValidate()
        {
            if (m_mainMenuScene != null)
                m_mainMenuSceneName = m_mainMenuScene.name;
        }
#endif
        [SerializeField] private string m_mainMenuSceneName;

        private void Awake()
        {
            if (s_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            s_Tokens = 0;
            m_waitTimeBeforeStart = new WaitForSeconds(m_timeBeforeStart);
            FindAnyObjectByType<BaseManager>().Initialise();
            FindAnyObjectByType<TowerSelectUI>().Initialise();
            FindAnyObjectByType<TowerUpgradeUI>().Initialise();
            FindAnyObjectByType<TowerCooldownUI>(FindObjectsInactive.Include).Initialise();
            FindAnyObjectByType<WaveManager>().Initialise();
            FindAnyObjectByType<EnemyManager>().Initialise();

            StartCoroutine(FadeMainMenu());
        }

        private void OnDestroy()
        {
            if (s_Instance != this) return;
            s_Instance = null;
            s_Tokens = 0;
        }

        private IEnumerator FadeMainMenu()
        {
            m_mainMenuCanvasGroup.alpha = 1;

            for (float i = 0; i < m_canvasGroupFadeTime; i += Time.deltaTime)
            {
                m_mainMenuCanvasGroup.alpha = Mathf.Lerp(1, 0, i / m_canvasGroupFadeTime);
                yield return null;
            }
            
            m_mainMenuCanvasGroup.alpha = 0;
            m_mainMenuCanvasGroup.gameObject.SetActive(false);

            yield return m_waitTimeBeforeStart;
            
            StartGame();
        }

        private static void StartGame()
        {
            WaveManager.s_Instance.StartWave();
        }

        public void StopGame()
        {
            SceneManager.LoadScene(m_mainMenuSceneName);
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