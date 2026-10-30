using FckAI.Audio;
using FckAI.Core;
using FckAI.Enemies;
using UnityEngine;

namespace FckAI.Towers.PaintBucket
{
    public class PaintArea : MonoBehaviour
    {
        [SerializeField] private BoxCollider m_boxCollider;
        [SerializeField] private MeshRenderer m_meshRenderer;
        
        private float m_damage;
        private float m_maxTime;
        private float m_currentTime;

        private Collider[] m_colliders;

        private Vector3 m_size;

        private bool m_isActive;

        public void Initialise(float maxTime)
        {
            gameObject.SetActive(false);

            float sizeX = m_boxCollider.size.x * transform.localScale.x;
            float sizeY = m_boxCollider.size.y * transform.localScale.y;
            float sizeZ = m_boxCollider.size.z * transform.localScale.z;

            m_size = new Vector3(sizeX, sizeY, sizeZ);
            m_maxTime = maxTime;

            m_colliders = new Collider[5];
        }
        
        public void Show(Vector3 position, float damage, Color splashColor)
        {
            transform.position = position;
            m_damage = damage;
            m_currentTime = m_maxTime;
            m_meshRenderer.material.color = splashColor;
            gameObject.SetActive(true);
            AudioManager.PlaySFX(AudioType.PaintLand);
            m_isActive = true;
            CheckForHit();
        }
        
        private void Update()
        {
            if (!m_isActive) return;
            m_currentTime -= Time.deltaTime;
            if (!(m_currentTime <= 0)) return;
            m_isActive = false;
            gameObject.SetActive(false);
        }

        private void CheckForHit()
        {
            int hitCount = Physics.OverlapBoxNonAlloc(transform.position + m_boxCollider.center, m_size, m_colliders,
                transform.rotation, GameManager.s_Instance.m_GameData.m_EnemyLayer);
            if (hitCount == 0) return;

            foreach (Collider collider in m_colliders)
            {
                if (collider == null)
                    continue;

                EnemyController enemy = collider.GetComponent<EnemyController>();
                enemy.Damage(m_damage);
            }
        }
    }
}