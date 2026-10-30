using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.Core
{
    public partial class BaseManager : MonoBehaviour
    {
        [AutoStaticsCleanup]
        public static BaseManager s_Instance;

        public int m_health { get; private set; }
        [SerializeField] private TMP_Text m_healthText;
        
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
            AddHealth(GameManager.s_Instance.m_GameData.m_StartHealth);
        }

        public void AddHealth(int health)
        {
            m_health += health;
            m_healthText.text = m_health.ToString();
            if (m_health < 0)
                m_health = 0;
            if (m_health == 0)
                GameManager.s_Instance.FinishGame();
        }
    }
}