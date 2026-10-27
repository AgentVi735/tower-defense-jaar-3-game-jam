using System.Collections;
using FckAI.Core;
using FckAI.UI.Towers;
using UnityEngine;

namespace FckAI.Towers
{
    public class Tower : MonoBehaviour
    {
        public TowerData m_Data { get; private set; }
        private int m_currentLevel;
        public TowerLevel m_NextLevel { get; private set; }
        [SerializeField] private MeshRenderer m_meshRenderer;

        [SerializeField] private Vector3 m_uiOffset;

        [SerializeField] private Color m_defaultColor = Color.white;
        [SerializeField] private Color m_hoverColor = Color.white;
        [SerializeField] private float m_hoverLerpTime = 0.3f;
        private Coroutine m_hoverLerpCoroutine;
        private float m_hoverIndex;

        private bool m_isHovering;
        private bool m_isSelected;

        private float m_currentDamage;
        private float m_currentCooldown;
        private float m_currentRange;

        public virtual void Initialise(TowerData data)
        {
            m_Data = data;
            m_currentDamage = m_Data.m_BaseDamage;
            m_currentCooldown = m_Data.m_BaseCooldown;
            m_currentRange = m_Data.m_BaseRange;
            m_currentLevel = 1;
            m_NextLevel = m_Data.m_NextLevel;
        }

        public virtual void Upgrade()
        {
            if (GameManager.s_Tokens < m_NextLevel.m_Cost)
                return;

            GameManager.s_Tokens -= m_NextLevel.m_Cost;
            
            m_currentLevel++;
            m_currentDamage = m_NextLevel.m_NewDamage;
            m_currentCooldown = m_NextLevel.m_NewCooldown;
            m_currentRange = m_NextLevel.m_NewRange;
            m_NextLevel = m_NextLevel.m_NextLevel;
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
            if (!m_isHovering)
                ToggleHover(false);
        }

        private void OnSelect()
        {
            if (!m_isHovering || m_isSelected) return;

            m_isSelected = true;
            Vector3 slotPos = m_uiOffset + transform.position;
            Vector2 uiPos = GameManager.s_Instance.m_Camera.WorldToScreenPoint(slotPos);
            TowerUpgradeUI.s_Instance.Show(uiPos, this);
        }
        
        private void ToggleHover(bool toggle)
        {
            m_isHovering = toggle;
            
            if (m_isSelected) return;
            
            if (m_hoverLerpCoroutine != null)
                StopCoroutine(m_hoverLerpCoroutine);

            if (m_hoverIndex > 0)
                m_hoverIndex = m_hoverLerpTime - m_hoverIndex;
            
            m_hoverLerpCoroutine = StartCoroutine(m_meshRenderer.materials.Length < 2
                ? HoverColorCoroutine(toggle)
                : HoverColorsCoroutine(toggle));
        }

        private IEnumerator HoverColorCoroutine(bool toggle)
        {
            Color startColor = m_meshRenderer.material.color;
            Color endColor = toggle ? m_hoverColor : m_defaultColor;
            
            m_meshRenderer.material.color = startColor;

            for (; m_hoverIndex < m_hoverLerpTime; m_hoverIndex += Time.deltaTime)
            {
                m_meshRenderer.material.color = Color.Lerp(startColor, endColor, m_hoverIndex / m_hoverLerpTime);
                yield return null;
            }

            m_hoverIndex = 0;
            
            m_meshRenderer.material.color = endColor;
        }
        
        private IEnumerator HoverColorsCoroutine(bool toggle)
        {
            Color startColor = m_meshRenderer.material.color;
            Color endColor = toggle ? m_hoverColor : m_defaultColor;
            
            foreach (Material material in m_meshRenderer.materials)
                material.color = startColor;

            for (; m_hoverIndex < m_hoverLerpTime; m_hoverIndex += Time.deltaTime)
            {
                Color newColor = Color.Lerp(startColor, endColor, m_hoverIndex / m_hoverLerpTime);
                foreach (Material material in m_meshRenderer.materials)
                    material.color = newColor;
                yield return null;
            }

            m_hoverIndex = 0;

            foreach (Material material in m_meshRenderer.materials)
                material.color = endColor;
        }
    }
}