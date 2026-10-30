#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FckAI.UI.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private MainMenuLogo m_mainMenuLogo;
        
#if UNITY_EDITOR
        [SerializeField] private SceneAsset m_gameScene;
        private void OnValidate()
        {
            if (m_gameScene != null)
                m_gameSceneName = m_gameScene.name;
        }
#endif
        [SerializeField] private string m_gameSceneName;

        public void StartGame()
        {
            m_mainMenuLogo.SavePositions();
            SceneManager.LoadScene(m_gameSceneName);
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }
    }
}