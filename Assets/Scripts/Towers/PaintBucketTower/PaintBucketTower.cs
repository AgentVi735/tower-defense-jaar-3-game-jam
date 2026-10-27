using System.Collections;
using System.Linq;
using FckAI.Core;
using FckAI.Enemies;
using UnityEngine;

namespace FckAI.Towers.PaintBucket
{
    public class PaintBucketTower : Tower
    {
        private new PaintBucketData m_Data;

        [SerializeField] private Transform m_shootTransform;

        private PaintProjectile m_projectile;
        private PaintArea[] m_areas;

        private EnemyController m_target;

        private Collider[] m_enemyCheckColl;

        private bool m_hasCooldown;

        [SerializeField] private LayerMask m_enemyLayer;

        public override void Initialise(TowerData data)
        {
            base.Initialise(data);
            m_Data = (PaintBucketData)base.m_Data;
            m_projectile = Instantiate(m_Data.m_PaintProjectilePrefab, transform.position, Quaternion.identity, transform);
            m_projectile.ChangeSpeed(m_currentProjectileSpeed);
            m_projectile.Initialise(this, m_shootTransform.position);
            m_areas = new PaintArea[m_Data.m_MaxPaintAreas];
            for (int i = 0; i < m_Data.m_MaxPaintAreas; i++)
            {
                PaintArea area = Instantiate(m_Data.m_PaintAreaPrefab, transform);
                area.Initialise(m_Data.m_PaintAreaTime, m_Data.m_PaintHitTick);
                m_areas[i] = area;
            }
            m_shootTransform.gameObject.SetActive(false);
            m_enemyCheckColl = new Collider[5];
        }

        private void Update()
        {
            if (m_hasCooldown) return;
            int amount = Physics.OverlapSphereNonAlloc(transform.position, m_Data.m_BaseRange, m_enemyCheckColl,
                GameManager.s_Instance.m_GameData.m_EnemyLayer);
            if (amount == 0) return;

            EnemyController enemy = m_enemyCheckColl[0].GetComponent<EnemyController>();
            m_target = enemy;
            Attack();
        }

        protected override void Attack()
        {
            m_projectile.Shoot(m_target);
            m_hasCooldown = true;
        }

        protected override IEnumerator StartCooldown()
        {
            m_hasCooldown = true;
            yield return m_waitCurrentCooldown;
            m_hasCooldown = false;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            m_projectile.ChangeSpeed(m_currentProjectileSpeed);
        }

        private PaintArea GetNewArea()
        {
            PaintArea area = m_areas.FirstOrDefault(paintArea => !paintArea.gameObject.activeInHierarchy);

            if (area == null)
            {
                Debug.LogWarning("Unable to pool a paint area");
                return null;
            }

            return area;
        }
        
        public void ShowNewArea(Vector3 position)
        {
            StartCoroutine(StartCooldown());

            PaintArea area = GetNewArea();

            if (area == null)
                return;
            
            area.Show(position, m_currentDamage);
        }
    }
}