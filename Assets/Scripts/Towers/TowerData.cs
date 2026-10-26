using UnityEngine;

namespace FckAI.Towers
{
    public class TowerData : ScriptableObject
    {
        [SerializeField] private string m_towerName;
        public string m_TowerName => m_towerName;
        [SerializeField] private float m_baseHealth;
        public float m_BaseHealth => m_baseHealth;
        [SerializeField] private float m_baseDamage;
        public float m_BaseDamage => m_baseDamage;
        [SerializeField] private Tower m_prefab;
        public Tower m_Prefab => m_prefab;

        private void Reset()
        {
            m_towerName = name;
        }
    }
}