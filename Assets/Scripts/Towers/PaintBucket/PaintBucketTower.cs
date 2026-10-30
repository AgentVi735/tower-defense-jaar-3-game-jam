using System.Linq;
using FckAI.Audio;
using FckAI.Core;
using FckAI.Enemies;
using UnityEngine;

namespace FckAI.Towers.PaintBucket
{
    public class PaintBucketTower : Tower
    {
        public new PaintBucketData m_Data { get; private set; }

        [SerializeField] private Transform m_shootTransform;

        private PaintProjectile m_projectile;
        private PaintArea[] m_areas;

        private EnemyController m_target;

        private Collider[] m_enemyCheckColl;

        public override void Initialise(TowerData data, Vector3 uiOffset)
        {
            base.Initialise(data, uiOffset);
            m_Data = (PaintBucketData)base.m_Data;
            m_projectile = Instantiate(m_Data.m_PaintProjectilePrefab, transform.position, Quaternion.identity, transform);
            m_projectile.Initialise(this, m_shootTransform.position);
            m_areas = new PaintArea[m_Data.m_MaxPaintAreas];
            for (int i = 0; i < m_Data.m_MaxPaintAreas; i++)
            {
                PaintArea area = Instantiate(m_Data.m_PaintAreaPrefab, transform);
                area.Initialise(m_Data.m_PaintAreaTime);
                m_areas[i] = area;
            }
            m_shootTransform.gameObject.SetActive(false);
            m_enemyCheckColl = new Collider[5];
            Vector3 rotation = Vector3.zero;
            rotation.y = Random.Range(0, 359);
            m_TowerTransform.rotation = Quaternion.Euler(rotation);
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

        private void Attack()
        {
            m_projectile.Shoot(m_target);
            if (m_isSelected)
                AudioManager.PlaySFX(AudioType.PaintShoot);
            StartCoroutine(StartCooldown());
        }

        private PaintArea GetNewArea()
        {
            PaintArea area = m_areas.FirstOrDefault(paintArea => !paintArea.gameObject.activeInHierarchy);

            if (area != null) return area;
            Debug.LogWarning("Unable to pool a paint area");
            return null;
        }
        
        public void ShowNewArea(Vector3 position, Color splashColor)
        {
            PaintArea area = GetNewArea();

            if (area == null)
                return;
            
            area.Show(position, m_currentDamage, splashColor);
        }
    }
}