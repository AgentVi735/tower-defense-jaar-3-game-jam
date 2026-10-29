using UnityEditor;
using UnityEngine;

namespace FckAI.Towers
{
    [CreateAssetMenu(menuName = "Towers/Level")]
    public class TowerLevel : ScriptableObject
    {
        [SerializeField] private TowerLevel m_nextLevel;
        public TowerLevel m_NextLevel => m_nextLevel;
        [SerializeField] private string m_description;
        public string m_Description => m_description;
        [SerializeField] private int m_cost;
        public int m_Cost => m_cost;
        [SerializeField] private float m_newDamage;
        public float m_NewDamage => m_newDamage;
        [SerializeField] private float m_newCooldown;
        public float m_NewCooldown => m_newCooldown;
        [SerializeField] private float m_newRange;
        public float m_NewRange => m_newRange;
        [SerializeField] private float m_newProjectileSpeed;
        public float m_NewProjectileSpeed => m_newProjectileSpeed;

        #if UNITY_EDITOR
        [SerializeField] private TowerLevel m_copyLevelStats;
        [SerializeField] private TowerData m_copyDataStats;

        private void OnValidate()
        {
            if (m_copyLevelStats != null)
                CopyLevelStats();
            else if (m_copyDataStats != null)
                CopyDataStats();
        }

        private void CopyLevelStats()
        {
            if (string.IsNullOrEmpty(m_description))
                m_description = m_copyLevelStats.m_Description;

            m_cost = m_copyLevelStats.m_Cost;
            m_newDamage = m_copyLevelStats.m_NewDamage;
            m_newCooldown = m_copyLevelStats.m_NewCooldown;
            m_newRange = m_copyLevelStats.m_NewRange;
            m_newProjectileSpeed = m_copyLevelStats.m_NewProjectileSpeed;
            
            EditorUtility.SetDirty(this);
            Debug.Log($"Copied data from {m_copyLevelStats.name} to level {name}.");
            m_copyLevelStats = null;
        }

        private void CopyDataStats()
        {
            m_cost = m_copyDataStats.m_BaseCost;
            m_newDamage = m_copyDataStats.m_BaseDamage;
            m_newCooldown = m_copyDataStats.m_BaseCooldown;
            m_newRange = m_copyDataStats.m_BaseRange;
            m_newProjectileSpeed = m_copyDataStats.m_BaseProjectileSpeed;

            EditorUtility.SetDirty(this);
            Debug.Log($"Copied data from {m_copyDataStats.name} to level {name}.");
            m_copyDataStats = null;
        }
        #endif
    }
}