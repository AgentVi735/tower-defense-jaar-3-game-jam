using System.Collections;
using UnityEngine;


namespace FckAI.Towers
{
    public class TowerSlotRenderer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        private Color m_defaultColor;
        [SerializeField] private Color m_hoverColor = Color.white;

        [SerializeField] private float m_hoverLerpTime;
        private float m_hoverCurrentTime;

        private Coroutine m_lerpCoroutine;

        private void Awake()
        {
            m_defaultColor = m_spriteRenderer.color;
            m_hoverCurrentTime = 0;
        }

        public void ToggleHover(bool toggle)
        {
            if (m_lerpCoroutine != null)
                StopCoroutine(m_lerpCoroutine);
            m_lerpCoroutine = StartCoroutine(ChangeColorCoroutine(toggle));
        }

        private IEnumerator ChangeColorCoroutine(bool toggle)
        {
            Color startColor = m_spriteRenderer.color;
            Color endColor = toggle ? m_hoverColor : m_defaultColor;

            m_spriteRenderer.color = startColor;
            
            if (m_hoverCurrentTime > 0)
                m_hoverCurrentTime = m_hoverLerpTime - m_hoverCurrentTime;

            for (; m_hoverCurrentTime < m_hoverLerpTime; m_hoverCurrentTime += Time.deltaTime)
            {
                m_spriteRenderer.color = Color.Lerp(startColor, endColor, m_hoverCurrentTime / m_hoverLerpTime);
                yield return null;
            }

            m_spriteRenderer.color = endColor;

            m_hoverCurrentTime = 0;
        }

        public void Toggle(bool toggle) => gameObject.SetActive(toggle);
    }
}