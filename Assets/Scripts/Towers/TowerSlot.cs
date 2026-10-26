using FckAI.Core;
using FckAI.UI.Towers;
using UnityEngine;

namespace FckAI.Towers
{
    public class TowerSlot : MonoBehaviour
    {
        [SerializeField] private TowerSlotRenderer m_slotRenderer;
        [SerializeField] private Vector3 m_uiOffset;

        private bool m_isHovering;
    
        private void OnMouseEnter() => OnHover(true);
        private void OnMouseExit() => OnHover(false);
        private void OnMouseUpAsButton() => OnPress();

        private void OnHover(bool toggle)
        {
            m_isHovering = toggle;
            m_slotRenderer.ToggleHover(toggle);
        }

        private void OnPress()
        {
            if (!m_isHovering)
                return;
            
            Vector3 slotPos = m_uiOffset + transform.position;
            Vector2 uiPos = GameManager.Instance.m_Camera.WorldToScreenPoint(slotPos);
            TowerSelectUI.s_Instance.Show(uiPos);
        }
    }
}