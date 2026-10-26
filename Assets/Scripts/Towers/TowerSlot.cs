using FckAI.Core;
using FckAI.UI.Towers;
using UnityEngine;

namespace FckAI.Towers
{
    public class TowerSlot : MonoBehaviour
    {
        [SerializeField] private TowerSlotRenderer m_slotRenderer;
        [SerializeField] private Vector3 m_uiOffset;
        [SerializeField] private Transform m_spawnTransform;
        [SerializeField] private BoxCollider m_boxCollider;

        private Tower m_tower;

        private bool m_hasTower => m_tower != null;

        private bool m_isHovering;
        private bool m_isSelected;
    
        private void OnMouseEnter() => OnHover(true);
        private void OnMouseExit() => OnHover(false);
        private void OnMouseUpAsButton() => OnPress();

        private void OnHover(bool toggle)
        {
            m_isHovering = toggle;
            if (m_isSelected) return;
            m_slotRenderer.ToggleHover(toggle);
        }

        public void OnCancelPlacement()
        {
            m_isSelected = false;
            if (!m_isHovering)
                OnHover(false);
        }

        private void OnPress()
        {
            if (!m_isHovering || m_isSelected) return;
            if (m_hasTower) return;

            m_isSelected = true;
            Vector3 slotPos = m_uiOffset + transform.position;
            Vector2 uiPos = GameManager.Instance.m_Camera.WorldToScreenPoint(slotPos);
            TowerSelectUI.s_Instance.Show(uiPos, this);
        }
        
        public void PlaceTower(TowerData data)
        {
            m_isSelected = false;
            GameManager.s_Tokens -= data.m_BaseCost;
            m_tower = Instantiate(data.m_Prefab, m_spawnTransform);
            m_tower.Initialise(data);
            m_slotRenderer.Toggle(false);
            m_boxCollider.enabled = false;
        }
    }
}