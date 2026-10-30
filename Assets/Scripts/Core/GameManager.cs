using System.Collections;
using FckAI.Core.Waves;
using FckAI.Enemies;
using FckAI.UI.MainMenu;
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
        
        [SerializeField] private float m_canvasGroupFadeTime;
        [SerializeField] private CanvasGroup m_mainMenuCanvasGroup;
        [SerializeField] private MainMenuLogo m_mainMenuLogo;
        [SerializeField] private CanvasGroup m_winCanvasGroup;
        [SerializeField] private CanvasGroup m_loseCanvasGroup;

        private bool hasFinished;

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

            StartCoroutine(FadeMainMenu(false));
        }

        private void OnDestroy()
        {
            if (s_Instance != this) return;
            s_Instance = null;
            s_Tokens = 0;
        }
        
        private IEnumerator FadeMainMenu(bool toggle)
        {
            float startAlpha = toggle ? 0 : 1;
            float endAlpha = toggle ? 1 : 0;
            m_mainMenuCanvasGroup.alpha = startAlpha;
            m_mainMenuLogo.gameObject.SetActive(!toggle);
            m_mainMenuCanvasGroup.gameObject.SetActive(true);
            
            for (float i = 0; i < m_canvasGroupFadeTime; i += Time.deltaTime)
            {
                m_mainMenuCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, i / m_canvasGroupFadeTime);
                yield return null;
            }
            
            m_mainMenuCanvasGroup.alpha = toggle ? 1 : 0;
            m_mainMenuCanvasGroup.gameObject.SetActive(toggle);
            
            if (toggle)
                GoToMainMenu();
            else
            {
                yield return m_waitTimeBeforeStart;
                StartGame();
            }
        }

        private IEnumerator FadeMenu()
        {
            float startAlpha = 0;
            float endAlpha = 1;
            bool hasWon = BaseManager.s_Instance.m_health > 0;
            CanvasGroup group;
            if (hasWon)
                group = m_winCanvasGroup;
            else
                group = m_loseCanvasGroup;
            group.alpha = startAlpha;
            group.gameObject.SetActive(true);
            
            for (float i = 0; i < m_canvasGroupFadeTime; i += Time.deltaTime)
            {
                group.alpha = Mathf.Lerp(startAlpha, endAlpha, i / m_canvasGroupFadeTime);
                yield return null;
            }
            
            group.alpha = 1;

            yield return new WaitForSeconds(5);
            
            StopGame();
        }

        private static void StartGame()
        {
            WaveManager.s_Instance.StartWave();
        }

        public void FinishGame()
        {
            if (hasFinished)
                return;
            hasFinished = true;
            StartCoroutine(FadeMenu());
        }
        
        public void StopGame()
        {
            m_mainMenuLogo.ResetPositions();
            StartCoroutine(FadeMainMenu(true));
        }

        private void GoToMainMenu()
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