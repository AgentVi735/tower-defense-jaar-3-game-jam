using System.Collections;
using FckAI.Audio;
using FckAI.Core;
using FckAI.UI.Towers;
using UnityEngine;

namespace FckAI.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        public TowerData m_Data { get; private set; }
        public TowerLevel m_NextLevel { get; private set; }
        [SerializeField] private MeshRenderer m_meshRenderer;
        [SerializeField] private Transform m_towerTransform;
        protected Transform m_TowerTransform => m_towerTransform;
        [SerializeField] private Transform m_rangeTransform;
        [SerializeField] private SpriteRenderer m_rangeRenderer;
        private float m_rangeAlpha;

        private Vector3 m_uiOffset;

        [SerializeField] private Color m_defaultColor = Color.white;
        [SerializeField] private Color m_hoverColor = Color.white;
        [SerializeField] private float m_hoverLerpTime = 0.3f;
        private Coroutine m_hoverLerpCoroutine;
        private float m_hoverIndex;

        private bool m_isHovering;
        protected bool m_isSelected;
        protected bool m_hasCooldown;
        public float m_currentCooldownProgress { get; private set; }

        public float m_currentDamage { get; private set; }
        public float m_currentCooldown { get; private set; }
        public float m_currentRange { get; private set; }
        public float m_currentProjectileSpeed { get; private set; }

        public virtual void Initialise(TowerData data, Vector3 uiOffset)
        {
            m_Data = data;
            m_currentDamage = m_Data.m_BaseDamage;
            m_currentCooldown = m_Data.m_BaseCooldown;
            m_currentRange = m_Data.m_BaseRange;
            m_currentProjectileSpeed = m_Data.m_BaseProjectileSpeed;
            m_NextLevel = m_Data.m_NextLevel;
            m_uiOffset = uiOffset;
            Color rangeColor = m_rangeRenderer.color;
            m_rangeAlpha = rangeColor.a;
            rangeColor.a = 0;
            m_rangeRenderer.color = rangeColor;
            AudioManager.PlaySFX(AudioType.TowerPlace);
        }
        
        public virtual void Upgrade()
        {
            if (GameManager.s_Tokens < m_NextLevel.m_Cost)
                return;

            GameManager.AddTokens(-m_NextLevel.m_Cost);

            m_currentDamage = m_NextLevel.m_NewDamage;
            m_currentCooldown = m_NextLevel.m_NewCooldown;
            m_currentRange = m_NextLevel.m_NewRange;
            m_rangeTransform.localScale = Vector3.one * (m_currentRange * 2);
            m_currentProjectileSpeed = m_NextLevel.m_NewProjectileSpeed;
            m_NextLevel = m_NextLevel.m_NextLevel;
            AudioManager.PlaySFX(AudioType.TowerUpgrade);
        }

        private void OnMouseEnter() => ToggleHover(true);

        private void OnMouseExit() => ToggleHover(false);

        private void OnMouseUpAsButton()
        {
            if (!m_isHovering || m_isSelected)
                return;
            
            OnSelect();
        }

        public void OnDeselect()
        {
            m_isSelected = false;
            AudioManager.PlaySFX(AudioType.TowerDeselect);
            if (!m_isHovering)
                ToggleHover(false);
        }

        private void OnSelect()
        {
            if (!m_isHovering || m_isSelected) return;

            if (TowerSelectUI.s_Instance.m_IsActive || TowerUpgradeUI.s_Instance.m_IsActive)
                return;

            m_isSelected = true;
            AudioManager.PlaySFX(AudioType.TowerSelect);
            Vector3 slotPos = m_uiOffset + transform.position;
            Vector2 uiPos = GameManager.s_Instance.m_Camera.WorldToScreenPoint(slotPos);
            TowerUpgradeUI.s_Instance.Show(uiPos, this);

            if (m_currentCooldownProgress > 0)
                TowerCooldownUI.s_Instance.Show(this);
        }
        
        private void ToggleHover(bool toggle)
        {
            m_isHovering = toggle;
            
            if (m_isSelected) return;
            
            if (m_hoverLerpCoroutine != null)
                StopCoroutine(m_hoverLerpCoroutine);

            if (m_hoverIndex > 0)
                m_hoverIndex = m_hoverLerpTime - m_hoverIndex;
            
            UpdateRangeDisplay();
            
            m_hoverLerpCoroutine = StartCoroutine(m_meshRenderer.materials.Length < 2
                ? HoverColorCoroutine(toggle)
                : HoverColorsCoroutine(toggle));
        }

        private IEnumerator HoverColorCoroutine(bool toggle)
        {
            Color startColor = m_meshRenderer.material.color;
            Color endColor = toggle ? m_hoverColor : m_defaultColor;
            Color rangeColor = m_rangeRenderer.color;
            float rangeStartAlpha = rangeColor.a;
            float rangeEndAlpha = toggle ? m_rangeAlpha : 0;

            m_rangeRenderer.color = rangeColor;
            m_rangeTransform.gameObject.SetActive(true);
            
            if (m_hoverIndex > 0)
                m_hoverIndex = m_hoverLerpTime - m_hoverIndex;
            
            for (; m_hoverIndex < m_hoverLerpTime; m_hoverIndex += Time.deltaTime)
            {
                float ratio = m_hoverIndex / m_hoverLerpTime;
                m_meshRenderer.material.color = Color.Lerp(startColor, endColor, ratio);
                rangeColor.a = Mathf.Lerp(rangeStartAlpha, rangeEndAlpha, ratio);
                m_rangeRenderer.color = rangeColor;
                yield return null;
            }

            m_hoverIndex = 0;
            
            m_meshRenderer.material.color = endColor;
            
            if (!toggle)
                m_rangeTransform.gameObject.SetActive(false);
        }
        
        private IEnumerator HoverColorsCoroutine(bool toggle)
        {
            Color startColor = m_meshRenderer.material.color;
            Color endColor = toggle ? m_hoverColor : m_defaultColor;
            Color rangeColor = m_rangeRenderer.color;
            float rangeStartAlpha = rangeColor.a;
            float rangeEndAlpha = toggle ? m_rangeAlpha : 0;

            m_rangeRenderer.color = rangeColor;
            m_rangeTransform.gameObject.SetActive(true);
            
            foreach (Material material in m_meshRenderer.materials)
                material.color = startColor;
            
            if (m_hoverIndex > 0)
                m_hoverIndex = m_hoverLerpTime - m_hoverIndex;

            for (; m_hoverIndex < m_hoverLerpTime; m_hoverIndex += Time.deltaTime)
            {
                float ratio = m_hoverIndex / m_hoverLerpTime;
                Color newColor = Color.Lerp(startColor, endColor, ratio);
                foreach (Material material in m_meshRenderer.materials)
                    material.color = newColor;
                rangeColor.a = Mathf.Lerp(rangeStartAlpha, rangeEndAlpha, ratio);
                m_rangeRenderer.color = rangeColor;
                yield return null;
            }

            m_hoverIndex = 0;

            foreach (Material material in m_meshRenderer.materials)
                material.color = endColor;
            
            if (!toggle)
                m_rangeTransform.gameObject.SetActive(false);
        }

        protected IEnumerator StartCooldown()
        {
            m_hasCooldown = true;

            m_currentCooldownProgress = 0;
            if (m_isSelected)
                TowerCooldownUI.s_Instance.Show(this);
            
            for (; m_currentCooldownProgress < m_currentCooldown; m_currentCooldownProgress += Time.deltaTime)
            {
                yield return null;
            }

            m_currentCooldownProgress = m_currentCooldown;
            
            m_hasCooldown = false;
        }

        private void UpdateRangeDisplay()
        {
            m_rangeTransform.localScale = Vector3.one * (m_currentRange * 2);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, m_Data.m_BaseRange);
        }
    }
}