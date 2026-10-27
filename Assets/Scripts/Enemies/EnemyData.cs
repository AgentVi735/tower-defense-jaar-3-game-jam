using UnityEngine;

namespace FckAI.Enemies
{
    [CreateAssetMenu(menuName = "Enemy/Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string m_enemyName;
        public string m_EnemyName => m_enemyName;
        [SerializeField] private EnemyController m_enemyPrefab;
        public EnemyController m_EnemyPrefab => m_enemyPrefab;
        [SerializeField] private uint m_maxEnemyCount;
        public uint m_MaxEnemyCount => m_maxEnemyCount;

        [SerializeField] private int m_damage;
        public int m_Damage => m_damage;
        [SerializeField] private float m_health;
        public float m_Health => m_health;
        [SerializeField] private float m_speed;
        public float m_Speed => m_speed;
    }
}