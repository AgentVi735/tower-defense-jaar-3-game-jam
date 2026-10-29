using FckAI.Towers;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.UI;

namespace FckAI.UI.Towers
{
    public partial class TowerCooldownUI : MonoBehaviour
    {
        [AutoStaticsCleanup]
        public static TowerCooldownUI s_Instance { get; private set; }

        [SerializeField] private GameObject m_cooldownImage;
        [SerializeField] private Image m_cooldownFillImage;

        public bool m_IsActive => gameObject.activeInHierarchy;

        private Tower m_currentTower;

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

        private void Update() => UpdateDisplay();

        public void Initialise()
        {
            if (s_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            gameObject.SetActive(false);
        }

        public void Show(Tower tower)
        {
            if (m_IsActive)
                return;

            m_currentTower = tower;

            UpdateDisplay();
            
            gameObject.SetActive(true);
        }

        public void TurnBackOn()
        {
            if (m_currentTower == null) return;
            UpdateDisplay();
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            if (!m_IsActive)
                return;
            
            gameObject.SetActive(false);
            if (!m_currentTower) return;
            m_currentTower.OnDeselect();
            m_currentTower = null;
        }

        private void UpdateDisplay()
        {
            float fillAmount = m_currentTower.m_currentCooldownProgress / m_currentTower.m_currentCooldown;
            fillAmount = 1 - fillAmount;
            if (fillAmount <= 0)
                Hide();
            else
            {
                m_cooldownFillImage.fillAmount = fillAmount;
                if (!m_cooldownImage.activeInHierarchy)
                    m_cooldownImage.SetActive(true);
            }
        }
    }
}