using System;
using UnityEditor;
using UnityEngine;

namespace FckAI.Enemies
{
    [CreateAssetMenu(menuName = "Enemy/Modifiers")]
    public class EnemyModifierData : ScriptableObject
    {
        [SerializeField] private EnemyData m_targetEnemy;
        public EnemyData m_TargetEnemy => m_targetEnemy;
        
        [SerializeField] private int m_newDamage;
        public int m_NewDamage => m_newDamage;
        [SerializeField] private float m_newHealth;
        public float m_NewHealth => m_newHealth;
        [SerializeField] private float m_newSpeed;
        public float m_NewSpeed => m_newSpeed;
        
        #if UNITY_EDITOR
        [SerializeField] private EnemyModifierData m_copyModifierStats;
        
        private void OnValidate()
        {
            if (m_targetEnemy != null && m_NewDamage == 0)
                CopyBaseStats();
            else if (m_copyModifierStats != null)
                CopyModStats();
        }
        
        private void CopyBaseStats()
        {
            m_newDamage = m_targetEnemy.m_Damage;
            m_newHealth = m_targetEnemy.m_Health;
            m_newSpeed = m_targetEnemy.m_Speed;

            EditorUtility.SetDirty(this);
            Debug.Log($"Copied data from {m_targetEnemy.name} to level {name}.");
        }
        
        private void CopyModStats()
        {
            m_newDamage = m_copyModifierStats.m_newDamage;
            m_newHealth = m_copyModifierStats.m_newHealth;
            m_newSpeed = m_copyModifierStats.m_newSpeed;

            EditorUtility.SetDirty(this);
            Debug.Log($"Copied data from {m_copyModifierStats.name} to level {name}.");
            m_copyModifierStats = null;
        }
#endif
    }
}