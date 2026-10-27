using UnityEngine;
using UnityEngine.AI;

namespace FckAI.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        private EnemyData m_data;

        private EnemyWaypoint m_currentWaypoint;

        private NavMeshPath m_path;
        private int m_pathIdx;
        
        public void Initialise(EnemyData data)
        {
            m_data = data;
            gameObject.SetActive(false);
        }

        public void EnableEnemy()
        {
            m_currentWaypoint = EnemyManager.s_Instance.m_StartingPoint;
            transform.position = m_currentWaypoint.transform.position;
            gameObject.SetActive(true);
            GetNewPath();
        }

        private void GetNewPath()
        {
            if (m_currentWaypoint.m_NextWaypoint == null)
            {
                EnemyManager.s_Instance.DisableEnemy(this);
                return;
            }
            m_currentWaypoint = m_currentWaypoint.m_NextWaypoint;
            bool couldHit = NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 3, 1);
            if (!couldHit)
            {
                Debug.LogError($"Could not get a position {transform.position} on NavMesh");
                return;
            }

            m_path = new NavMeshPath();
            NavMesh.CalculatePath(hit.position, m_currentWaypoint.transform.position, 1, m_path);
            m_pathIdx = 0;
        }

        public void Move()
        {
            if (!gameObject.activeInHierarchy)
                return;
            
            Vector3 posToMoveToGlobal = m_path.corners[m_pathIdx];
            Vector3 posToMoveToLocal = posToMoveToGlobal - transform.position;

            Vector3 movePos = posToMoveToLocal.normalized * (m_data.m_Speed * Time.deltaTime);

            transform.Translate(movePos);
        
            Vector3 diffPos = transform.position;
            diffPos.y = posToMoveToGlobal.y;
            float distance = Vector3.Distance(posToMoveToGlobal, diffPos);
        
            if (!(distance < 0.5f)) return;
            m_pathIdx++;
            if (m_path.corners.Length > m_pathIdx) return;
            GetNewPath();
        }
    }
}