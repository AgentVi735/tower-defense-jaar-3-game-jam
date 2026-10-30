using System.Collections;
using System.Linq;
using FckAI.Audio;
using FckAI.Core;
using FckAI.Enemies;
using FckAI.UI.Towers;
using UnityEngine;

namespace FckAI.Towers.PencilSharpener
{
    public class PencilSharpenerTower : Tower
    {
        private new PencilSharpenerData m_Data { get; set; }

        [SerializeField] private Transform m_shootTransform;
        [SerializeField] private Transform m_handleTransform;

        private PencilProjectile[] m_projectiles;

        private EnemyController m_target;

        private Collider[] m_enemyCheckColl;

        public override void Initialise(TowerData data, Vector3 uiOffset)
        {
            base.Initialise(data, uiOffset);
            m_Data = (PencilSharpenerData)base.m_Data;
            m_projectiles = new PencilProjectile[m_Data.m_MaxPencilProjectiles];
            for (int i = 0; i < m_Data.m_MaxPencilProjectiles; i++)
            {
                PencilProjectile projectile = Instantiate(m_Data.m_PencilProjectilePrefab, transform.position, Quaternion.identity, transform);
                projectile.Initialise(this, m_shootTransform.position);
                m_projectiles[i] = projectile;
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

        private void Attack()
        {
            PencilProjectile projectile = GetNewProjectile();
            if (projectile == null)
                return;
            
            projectile.Shoot(m_target);
            if (m_isSelected)
                AudioManager.PlaySFX(AudioType.PencilShoot);
            Vector3 lookPos = m_target.transform.position;
            lookPos.y = transform.position.y;
            m_TowerTransform.LookAt(lookPos);
            StartCoroutine(StartCooldown());
        }
        
        protected override IEnumerator StartCooldown()
        {
            m_hasCooldown = true;

            m_handleTransform.rotation = Quaternion.identity;
            m_currentCooldownProgress = 0;
            if (m_isSelected)
                TowerCooldownUI.s_Instance.Show(this);
            
            for (; m_currentCooldownProgress < m_currentCooldown; m_currentCooldownProgress += Time.deltaTime)
            {
                float yRotation = Mathf.Lerp(0, 360, m_currentCooldownProgress / m_currentCooldown);
                m_handleTransform.rotation = Quaternion.Euler(0, 0, yRotation);
                yield return null;
            }

            m_currentCooldownProgress = m_currentCooldown;
            m_handleTransform.rotation = Quaternion.identity;
            
            m_hasCooldown = false;
        }

        private PencilProjectile GetNewProjectile()
        {
            PencilProjectile projectile = m_projectiles.FirstOrDefault(projectile => !projectile.gameObject.activeInHierarchy);

            if (projectile != null) return projectile;
            Debug.LogWarning("Unable to pool a pencil projectile");
            return null;
        }
    }
}