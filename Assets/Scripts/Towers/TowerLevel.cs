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
    }
}