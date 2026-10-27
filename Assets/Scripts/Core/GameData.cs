using FckAI.Core.Waves;
using FckAI.Enemies;
using FckAI.Towers;
using UnityEngine;

namespace FckAI.Core
{
    [CreateAssetMenu(menuName = "Game Data")]
    public class GameData : ScriptableObject
    {
        [SerializeField] private TowerData[] m_towers;
        public TowerData[] m_Towers => m_towers;
        [SerializeField] private WaveData m_startWave;
        public WaveData m_StartWave => m_startWave;
        [SerializeField] private EnemyData[] m_enemyOrder;
        public EnemyData[] m_EnemyOrder => m_enemyOrder;
        [SerializeField] private LayerMask m_enemyLayer;
        public LayerMask m_EnemyLayer => m_enemyLayer;
    }
}