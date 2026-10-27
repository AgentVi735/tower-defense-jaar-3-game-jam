using UnityEngine;

namespace FckAI.Enemies
{
    public class EnemyWaypoint : MonoBehaviour
    {
        [SerializeField] private EnemyWaypoint m_nextWaypoint;
        public EnemyWaypoint m_NextWaypoint => m_nextWaypoint;
    }
}