using FckAI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FckAI.UI.Towers
{
    public class TowerUpgradeButton : Button
    {
        [SerializeField] private TMP_Text m_costText;
        private int m_upgradeCost;
        [SerializeField] private string m_maxLevelText;
        
        public void Initialise(int cost)
        {
            m_upgradeCost = cost;
            m_costText.text = $"{m_upgradeCost} Tokens";
            RefreshAvailability();
        }

        public void MaxLevelReached()
        {
            interactable = false;
            m_costText.text = m_maxLevelText;
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            Press();
            
            base.OnPointerClick(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            Press();
            base.OnSubmit(eventData);
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;
            
            TowerUpgradeUI.s_Instance.OnSelectButton();
        }

        public void RefreshAvailability()
        {
            interactable = GameManager.s_Tokens >= m_upgradeCost;
        }
    }
}