using FckAI.Audio;
using FckAI.Core;
using FckAI.Towers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FckAI.UI.Towers
{
    public class TowerSelectButton : Button
    {
        public TowerData m_TowerData { get; private set; }
        
        [SerializeField] private TMP_Text m_text;
        [SerializeField] private TMP_Text m_priceText;
        [SerializeField] private Image m_image;
        
        public void Initialise(TowerData data)
        {
            m_TowerData = data;
            m_text.text = data.m_TowerName;
            m_priceText.text = $"{data.m_BaseCost} Tokens";
            m_image.sprite = data.m_UiSprite;
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
            
            TowerSelectUI.s_Instance.OnSelectButton(m_TowerData);
            AudioManager.PlaySFX(AudioType.UIClick);
        }

        public void RefreshAvailability()
        {
            interactable = GameManager.s_Tokens >= m_TowerData.m_BaseCost;
        }
    }
}