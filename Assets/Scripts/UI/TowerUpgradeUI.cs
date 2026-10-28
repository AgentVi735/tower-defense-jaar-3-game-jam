using FckAI.Towers;
using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.UI.Towers
{
    public partial class TowerUpgradeUI : MonoBehaviour
    {
        [AutoStaticsCleanup]
        public static TowerUpgradeUI s_Instance { get; private set; }

        public bool m_IsActive => gameObject.activeInHierarchy;

        [SerializeField] private TMP_Text m_nameText;
        [SerializeField] private TMP_Text m_descriptionText;
        [SerializeField] private string m_maxLevelDescription;

        [SerializeField] private TowerUpgradeButton m_upgradeButton;
        [SerializeField] private GameObject m_deselectButton;
        private Tower m_currentTower;

        private void Awake()
        {
            if (s_Instance == null) return;
            if (s_Instance == this) return;
            Destroy(gameObject);
        }

        public void Initialise()
        {
            if (s_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            gameObject.SetActive(false);
            m_deselectButton.SetActive(false);
        }

        private void OnDestroy()
        {
            if (s_Instance == this)
                s_Instance = null;
        }

        public void Show(Vector3 position, Tower tower)
        {
            if (m_IsActive)
                return;

            m_currentTower = tower;
            transform.position = position;

            UpdateDisplay();
            
            gameObject.SetActive(true);
            m_deselectButton.SetActive(true);
        }

        public void Cancel()
        {
            Hide();
        }
        
        private void Hide()
        {
            if (!m_IsActive)
                return;
            
            gameObject.SetActive(false);
            m_deselectButton.SetActive(false);
            if (!m_currentTower) return;
            m_currentTower.OnDeselect();
            m_currentTower = null;
        }
        
        public void OnSelectButton()
        {
            m_currentTower.Upgrade();
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            m_nameText.text = m_currentTower.m_Data.m_TowerName;
            if (m_currentTower.m_NextLevel != null)
            {
                m_descriptionText.text = m_currentTower.m_NextLevel.m_Description;
                m_upgradeButton.Initialise(m_currentTower.m_NextLevel.m_Cost);
            }
            else
            {
                m_descriptionText.text = m_maxLevelDescription;
                m_upgradeButton.MaxLevelReached();
            }
        }

        public void RefreshAvailability()
        {
            m_upgradeButton.RefreshAvailability();
        }
    }
}