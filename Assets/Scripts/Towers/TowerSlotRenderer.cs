using System.Collections;
using UnityEngine;


namespace FckAI.Towers
{
    public class TowerSlotRenderer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private Color m_defaultColor = Color.white;
        [SerializeField] private Color m_hoverColor = Color.white;

        [SerializeField] private float m_hoverLerpTime;

        private Coroutine m_lerpCoroutine;

        private void Awake()
        {
            m_spriteRenderer.color = m_defaultColor;
        }

        public void ToggleHover(bool toggle)
        {
            if (m_lerpCoroutine != null)
                StopCoroutine(m_lerpCoroutine);
            m_lerpCoroutine = StartCoroutine(ChangeColorCoroutine(toggle));
        }

        private IEnumerator ChangeColorCoroutine(bool toggle)
        {
            Color startColor = toggle ? m_defaultColor : m_hoverColor;
            Color endColor = toggle ? m_hoverColor : m_defaultColor;

            m_spriteRenderer.color = startColor;

            for (float i = 0; i < m_hoverLerpTime; i += Time.deltaTime)
            {
                m_spriteRenderer.color = Color.Lerp(startColor, endColor, i / m_hoverLerpTime);
                yield return null;
            }

            m_spriteRenderer.color = endColor;
        }

        public void Toggle(bool toggle) => gameObject.SetActive(toggle);
    }
}