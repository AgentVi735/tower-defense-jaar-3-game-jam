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
    }
}