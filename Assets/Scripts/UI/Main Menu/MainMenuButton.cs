using System.Collections;
using FckAI.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FckAI.UI.MainMenu
{
    public class MainMenuButton : Button
    {
        [SerializeField] private RectTransform m_rectTransform;
        [SerializeField] private Vector3 m_defaultScale = Vector3.one;
        [SerializeField] private Vector3 m_hoverScale = Vector3.one;
        [SerializeField] private float m_scaleTime;
        private float m_scaleLerpIndex;

        private Coroutine m_lerpCoroutine;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            ChangeScale(true);
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            ChangeScale(false);
            base.OnPointerExit(eventData);
        }

        private IEnumerator LerpScaleCoroutine(bool toggle)
        {
            Vector3 startScale = m_rectTransform.localScale;
            Vector3 endScale = toggle ? m_hoverScale : m_defaultScale;

            if (m_scaleLerpIndex > 0)
                m_scaleLerpIndex = m_scaleTime - m_scaleLerpIndex;
            
            for (; m_scaleLerpIndex < m_scaleTime; m_scaleLerpIndex += Time.deltaTime)
            {
                m_rectTransform.localScale = Vector3.Lerp(startScale, endScale, m_scaleLerpIndex / m_scaleTime);
                yield return null;
            }

            m_scaleLerpIndex = 0;

            m_rectTransform.localScale = endScale;
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
            AudioManager.PlaySFX(AudioType.UIClick);
            if (m_lerpCoroutine != null)
                StopCoroutine(m_lerpCoroutine);
            m_rectTransform.localScale = m_defaultScale;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            ChangeScale(true);
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            ChangeScale(false);
            base.OnDeselect(eventData);
        }

        private void ChangeScale(bool toggle)
        {
            if (m_lerpCoroutine != null)
                StopCoroutine(m_lerpCoroutine);
            m_lerpCoroutine = StartCoroutine(LerpScaleCoroutine(toggle));
        }
    }
}