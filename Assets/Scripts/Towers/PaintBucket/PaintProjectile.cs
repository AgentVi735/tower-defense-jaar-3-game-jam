using System.Collections;
using FckAI.Enemies;
using UnityEngine;

namespace FckAI.Towers.PaintBucket
{
    public class PaintProjectile : MonoBehaviour
    {
        private PaintBucketTower m_tower;
        
        private Vector3 m_startPosition;
        private EnemyController m_target;
        
        private float m_speed;

        [SerializeField] private MeshRenderer m_meshRenderer;
        [SerializeField] private float m_percentageOfTimeToGoUp;

        public void Initialise(PaintBucketTower tower, Vector3 shootPosition)
        {
            gameObject.SetActive(false);
            m_tower = tower;
            m_startPosition = shootPosition;
        }
        
        public void Shoot(EnemyController enemyTrans)
        {
            m_target = enemyTrans;
            m_meshRenderer.material.color =
                m_tower.m_Data.m_PaintColors[Random.Range(0, m_tower.m_Data.m_PaintColors.Length)];
            m_speed = m_tower.m_currentProjectileSpeed;
            gameObject.SetActive(true);
            StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine()
        {
            transform.position = m_startPosition;

            bool isGoingUp = true;
            float timeToGoUp = m_speed * m_percentageOfTimeToGoUp;
            float timeToGoDown = m_speed - timeToGoUp;
            Vector3 currentTargetPos = m_target.transform.position;
            float distance = Vector3.Distance(m_startPosition, m_target.transform.position);
            for (float i = 0; i < m_speed; i += Time.deltaTime)
            {
                if (m_target.gameObject.activeInHierarchy && distance <= m_tower.m_Data.m_BaseRange)
                    currentTargetPos = m_target.transform.position;
                
                Vector3 newPos = Vector3.Lerp(m_startPosition, currentTargetPos, i / m_speed);
                if (isGoingUp)
                {
                    if (i >= timeToGoUp)
                        isGoingUp = false;
                }

                float yPos = isGoingUp
                    ? Mathf.Lerp(m_startPosition.y, currentTargetPos.y, i / timeToGoUp)
                    : Mathf.Lerp(m_startPosition.y, currentTargetPos.y, i - timeToGoUp / timeToGoDown);

                newPos.y = yPos;
                transform.position = newPos;
                yield return null;
            }

            m_tower.ShowNewArea(currentTargetPos, m_meshRenderer.material.color);
            gameObject.SetActive(false);
        }
    }
}
