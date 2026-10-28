using System.Collections.Generic;
using FckAI.Enemies;
using UnityEngine;

namespace FckAI.Core.Waves
{
    [CreateAssetMenu(menuName = "Waves/Data")]
    public class WaveData : ScriptableObject
    {
        [SerializeField] private WaveData m_nextWave;
        public WaveData m_NextWave => m_nextWave;
        
        [SerializeField] private int m_extraStartTokens;
        public int m_ExtraStartTokens => m_extraStartTokens;

        [SerializeField] private Dictionary<EnemyData, int> m_enemyCount;
        public Dictionary<EnemyData, int> m_EnemyCount => m_enemyCount;
        
        [SerializeField] private Dictionary<EnemyData, EnemyModifierData> m_enemyModifiers;

        public EnemyModifierData GetEnemyModifier(EnemyData enemy)
        {
            if (!m_enemyModifiers.ContainsKey(enemy)) return null;

            bool couldGetValue = m_enemyModifiers.TryGetValue(enemy, out EnemyModifierData modifier);
            if (couldGetValue) return modifier;

            Debug.LogError($"Unable to get modifier of enemy {enemy} in wave {name}");
            return null;
        }
    }
}