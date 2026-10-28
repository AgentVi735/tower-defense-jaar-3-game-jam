using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace FckAI.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyData m_Data { get; private set; }
        private EnemyModifierData m_currentModifier;

        public EnemyWaypoint m_CurrentWaypoint { get; private set; }

        public NavMeshPath m_Path { get; private set; }
        public int m_PathIdx { get; private set; }

        private float m_speed;
        private float m_health;

        public float GetSpeed() => m_currentModifier != null ? m_currentModifier.m_NewSpeed : m_Data.m_Speed;
        public float GetHealth() => m_currentModifier != null ? m_currentModifier.m_NewHealth : m_Data.m_Health;

        [SerializeField] private MeshRenderer m_meshRenderer;
        
        private Color m_defaultColor;
        [SerializeField] private Color m_hitColor = Color.white;
        [SerializeField] private float m_hitTime;
        private WaitForSeconds m_waitHitTime;
        private Coroutine m_hitCoroutine;

        public void Initialise(EnemyData data)
        {
            m_Data = data;
            gameObject.SetActive(false);
            m_defaultColor = m_meshRenderer.material.color;
            m_waitHitTime = new WaitForSeconds(m_hitTime);
        }

        public void SetModifier(EnemyModifierData newModifier)
        {
            m_currentModifier = newModifier;
            m_speed = GetSpeed();
            m_health = GetHealth();
        }
        
        public void EnableEnemy()
        {
            m_CurrentWaypoint = EnemyManager.s_Instance.m_StartingPoint;
            transform.position = m_CurrentWaypoint.transform.position;
            m_speed = GetSpeed();
            m_health = GetHealth();
            gameObject.SetActive(true);
            GetNewPath();
        }

        private void GetNewPath()
        {
            if (m_CurrentWaypoint.m_NextWaypoint == null)
            {
                EnemyManager.s_Instance.DisableEnemy(this);
                return;
            }
            m_CurrentWaypoint = m_CurrentWaypoint.m_NextWaypoint;
            bool couldHit = NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 3, 1);
            if (!couldHit)
            {
                Debug.LogError($"Could not get a position {transform.position} on NavMesh");
                return;
            }

            m_Path = new NavMeshPath();
            NavMesh.CalculatePath(hit.position, m_CurrentWaypoint.transform.position, 1, m_Path);
            m_PathIdx = 0;
        }

        public void Move()
        {
            if (!gameObject.activeInHierarchy)
                return;
            
            Vector3 posToMoveToGlobal = m_Path.corners[m_PathIdx];
            Vector3 posToMoveToLocal = posToMoveToGlobal - transform.position;

            Vector3 movePos = posToMoveToLocal.normalized * (m_speed * Time.deltaTime);

            transform.Translate(movePos);
        
            Vector3 diffPos = transform.position;
            diffPos.y = posToMoveToGlobal.y;
            float distance = Vector3.Distance(posToMoveToGlobal, diffPos);
        
            if (!(distance < 0.5f)) return;
            m_PathIdx++;
            if (m_Path.corners.Length > m_PathIdx) return;
            GetNewPath();
        }

        public void Damage(float damage)
        {
            if (m_health <= 0)
                return;
            m_health -= damage;
            if (m_health <= 0)
                EnemyManager.s_Instance.DisableEnemy(this);
            else
                StartHitCoroutine();
        }

        private void StartHitCoroutine()
        {
            if (m_hitCoroutine != null)
                StopCoroutine(m_hitCoroutine);
            if (gameObject.activeInHierarchy)
                m_hitCoroutine = StartCoroutine(HitCoroutine());
        }
        
        private IEnumerator HitCoroutine()
        {
            m_meshRenderer.material.color = m_hitColor;
            yield return m_waitHitTime;
            m_meshRenderer.material.color = m_defaultColor;
        }
    }
}