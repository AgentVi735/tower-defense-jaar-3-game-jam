using UnityEngine;

namespace FckAI.Towers
{
    public class TowerData : ScriptableObject
    {
        [SerializeField] private string m_towerName;
        public string m_TowerName => m_towerName;
        [SerializeField] private TowerLevel m_nextLevel;
        public TowerLevel m_NextLevel => m_nextLevel;
        [SerializeField] private int m_baseCost;
        public int m_BaseCost => m_baseCost;
        [SerializeField] private float m_baseDamage;
        public float m_BaseDamage => m_baseDamage;
        [SerializeField] private float m_baseCooldown;
        public float m_BaseCooldown => m_baseCooldown;
        [SerializeField] private float m_baseRange;
        public float m_BaseRange => m_baseRange;
        [SerializeField] private float m_baseProjectileSpeed;
        public float m_BaseProjectileSpeed => m_baseProjectileSpeed;
        [SerializeField] private Tower m_prefab;
        public Tower m_Prefab => m_prefab;

        private void Reset()
        {
            m_towerName = name;
        }
    }
}