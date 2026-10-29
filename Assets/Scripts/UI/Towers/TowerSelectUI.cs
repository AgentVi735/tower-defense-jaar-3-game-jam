using FckAI.Towers;
using FckAI.Core;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.UI.Towers
{
    public partial class TowerSelectUI : MonoBehaviour
    {
        [AutoStaticsCleanup]
        public static TowerSelectUI s_Instance { get; private set; }

        public bool m_IsActive => gameObject.activeInHierarchy;
        
        [SerializeField] private Transform m_buttonParent;
        [SerializeField] private TowerSelectButton m_buttonPrefab;
        private TowerSelectButton[] m_buttons;
        [SerializeField] private GameObject m_deselectButton;
        private TowerSlot m_currentSlot;

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

        public void Initialise()
        {
            if (s_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            SpawnButtons();
            gameObject.SetActive(false);
            m_deselectButton.SetActive(false);
        }
        
        private void SpawnButtons()
        {
            TowerData[] towers = GameManager.s_Instance.m_GameData.m_Towers;
            int count = towers.Length;
            m_buttons = new TowerSelectButton[count];

            for (int i = 0; i < count; i++)
            {
                TowerSelectButton button = Instantiate(m_buttonPrefab, m_buttonParent);
                button.Initialise(towers[i]);
                m_buttons[i] = button;
            }
        }

        public void Show(Vector3 position, TowerSlot slot)
        {
            if (m_IsActive)
                return;

            m_currentSlot = slot;
            transform.position = position;

            RefreshAvailability();
            
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
            if (!m_currentSlot) return;
            m_currentSlot.OnCancelPlacement();
            m_currentSlot = null;
        }
        
        public void OnSelectButton(TowerData data)
        {
            m_currentSlot.PlaceTower(data);
            m_currentSlot = null;
            Hide();
        }

        public void RefreshAvailability()
        {
            foreach (TowerSelectButton button in m_buttons)
                button.RefreshAvailability();
        }
    }
}