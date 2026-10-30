using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.UI.MainMenu
{
    public partial class MainMenuLogo : MonoBehaviour
    {
        [SerializeField] private RectTransform[] m_logosRectTransforms;
        private Vector2[] m_defaultPositions;
        [AutoStaticsCleanup]
        private static Vector2[] m_logosPosition;
        [AutoStaticsCleanup]
        private static float[] m_logosRotation;

        [SerializeField] private bool m_isActualMainMenu;

        private void Awake()
        {
            if (m_isActualMainMenu)
            {
                m_logosPosition = new Vector2[m_logosRectTransforms.Length];
                m_logosRotation = new float[m_logosRectTransforms.Length];
            }
            else
                LoadPositions();
        }

        private void LoadPositions()
        {
            m_defaultPositions = new Vector2[m_logosRectTransforms.Length];
            for (int i = 0; i < m_logosRectTransforms.Length; i++)
            {
                RectTransform rect = m_logosRectTransforms[i];
                m_defaultPositions[i] = rect.anchoredPosition;
                Vector2 pos = m_logosPosition[i];
                float zRot = m_logosRotation[i];
                rect.anchoredPosition = pos;
                rect.rotation = Quaternion.Euler(0, 0, zRot);
            }
        }

        public void SavePositions()
        {
            for (int i = 0; i < m_logosRectTransforms.Length; i++)
            {
                RectTransform rect = m_logosRectTransforms[i];
                m_logosPosition[i] = rect.anchoredPosition;
                m_logosRotation[i] = rect.rotation.eulerAngles.z;
            }
        }

        public void ResetPositions()
        {
            for (int i = 0; i < m_logosRectTransforms.Length; i++)
            {
                RectTransform rect = m_logosRectTransforms[i];
                rect.anchoredPosition = m_defaultPositions[i];
                rect.rotation = Quaternion.identity;
            }
        }
    }
}