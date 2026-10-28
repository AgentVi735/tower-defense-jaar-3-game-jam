using System.Collections;
using FckAI.Enemies;
using UnityEngine;

namespace FckAI.Towers.PencilSharpener
{
    public class PencilProjectile : MonoBehaviour
    {
        private PencilSharpenerTower m_tower;
        
        private Vector3 m_startPosition;
        private EnemyController m_target;
        
        private float m_speed;
        private float m_damage;

        [SerializeField] private MeshRenderer m_meshRenderer;

        public void Initialise(PencilSharpenerTower tower, Vector3 shootPosition)
        {
            gameObject.SetActive(false);
            m_tower = tower;
            m_startPosition = shootPosition;
        }
        
        public void Shoot(EnemyController enemyTrans)
        {
            m_target = enemyTrans;
            m_speed = m_tower.m_currentProjectileSpeed;
            m_damage = m_tower.m_currentDamage;
            gameObject.SetActive(true);
            StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine()
        {
            transform.position = m_startPosition;

            Vector3 currentTargetPos = m_target.transform.position;
            float distance = Vector3.Distance(m_startPosition, m_target.transform.position);
            for (float i = 0; i < m_speed; i += Time.deltaTime)
            {
                if (m_target.gameObject.activeInHierarchy && distance <= m_tower.m_Data.m_BaseRange)
                    currentTargetPos = m_target.transform.position;
                
                Vector3 newPos = Vector3.Lerp(m_startPosition, currentTargetPos, i / m_speed);
                newPos.y = 0;
                transform.position = newPos;

                transform.LookAt(currentTargetPos);
                
                yield return null;
            }

            if (m_target.gameObject.activeInHierarchy && distance <= m_tower.m_Data.m_BaseRange)
                m_target.Damage(m_damage);
            gameObject.SetActive(false);
        }
    }
}
