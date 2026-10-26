using FckAI.Towers;
using UnityEngine;

namespace FckAI.Core
{
    [CreateAssetMenu(menuName = "Game Data")]
    public class GameData : ScriptableObject
    {
        [SerializeField] private TowerData[] m_towers;
        public TowerData[] m_Towers => m_towers;
    }
}