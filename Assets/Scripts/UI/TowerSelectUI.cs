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

        [SerializeField] private Transform m_buttonParent;
        [SerializeField] private TowerSelectButton m_buttonPrefab;
        private TowerSelectButton[] m_buttons;

        private void Awake()
        {
            if (s_Instance == null) return;
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
            SpawnButtons();
            gameObject.SetActive(false);
        }
        
        private void SpawnButtons()
        {
            // Debug.Log(GameManager.Instance?.m_GameData?.m_Towers?.Length);
            TowerData[] towers = GameManager.Instance.m_GameData.m_Towers;
            int count = towers.Length;
            m_buttons = new TowerSelectButton[count];

            for (int i = 0; i < count; i++)
            {
                TowerSelectButton button = Instantiate(m_buttonPrefab, m_buttonParent);
                button.Initialise(towers[i]);
                m_buttons[i] = button;
            }
        }

        private void OnDestroy()
        {
            if (s_Instance == this)
                s_Instance = null;
        }

        public void Show(Vector3 position)
        {
            if (gameObject.activeInHierarchy)
                return;
            
            transform.position = position;
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            if (!gameObject.activeInHierarchy)
                return;
            
            gameObject.SetActive(false);
        }
        
        public void OnSelectButton(TowerData data)
        {
            Hide();
        }
    }
}