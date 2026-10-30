using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.UI.MainMenu
{
    public partial class MainMenuLogo : MonoBehaviour
    {
        [SerializeField] private RectTransform[] m_logosRectTransforms;
        [AutoStaticsCleanup]
        private static Vector2[] m_logosPosition;

        [SerializeField] private bool m_isActualMainMenu;

        private void Awake()
        {
            if (m_isActualMainMenu)
                m_logosPosition = new Vector2[m_logosRectTransforms.Length];
            else
                LoadPositions();
        }

        private void LoadPositions()
        {
            for (int i = 0; i < m_logosRectTransforms.Length; i++)
            {
                RectTransform rect = m_logosRectTransforms[i];
                Vector2 pos = m_logosPosition[i];
                rect.anchoredPosition = pos;
            }
        }

        public void SavePositions()
        {
            for (int i = 0; i < m_logosRectTransforms.Length; i++)
            {
                RectTransform rect = m_logosRectTransforms[i];
                m_logosPosition[i] = rect.anchoredPosition;
            }
        }

        public void ResetPositions()
        {
            foreach (RectTransform rect in m_logosRectTransforms)
                rect.anchoredPosition = Vector2.zero;
        }
    }
}