using UnityEngine;

namespace FckAI.Towers
{
    public class Tower : MonoBehaviour
    {
        public TowerData m_Data { get; private set; }
        private int m_currentLevel;
        private TowerLevel m_nextLevel;

        private float m_currentDamage;
        private float m_currentCooldown;
        private float m_currentRange;

        public virtual void Initialise(TowerData data)
        {
            m_Data = data;
            m_currentDamage = m_Data.m_BaseDamage;
            m_currentCooldown = m_Data.m_BaseCooldown;
            m_currentRange = m_Data.m_BaseRange;
            m_currentLevel = 1;
            m_nextLevel = m_Data.m_NextLevel;
        }

        public virtual void Upgrade()
        {
            m_currentLevel++;
            m_currentDamage = m_nextLevel.m_NewDamage;
            m_currentCooldown = m_nextLevel.m_NewCooldown;
            m_currentRange = m_nextLevel.m_NewRange;
            m_nextLevel = m_Data.m_NextLevel;
        }
    }
}