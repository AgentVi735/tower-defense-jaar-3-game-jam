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
        
        public void Initialise(TowerData data)
        {
            m_TowerData = data;
            m_text.text = data.m_TowerName;
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
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
            TowerSelectUI.s_Instance.OnSelectButton(m_TowerData);
        }
    }
}