using UnityEngine;

namespace FckAI.Towers
{
    public class Tower : MonoBehaviour
    {
        private TowerData m_data;
        private int m_currentLevel;
        private TowerLevel m_nextLevel;

        private float m_currentDamage;
        private float m_currentCooldown;
        private float m_currentRange;

        public virtual void Initialise(TowerData data)
        {
            m_data = data;
        }
    }
}