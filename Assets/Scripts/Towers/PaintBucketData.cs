using UnityEngine;

namespace FckAI.Towers
{
    [CreateAssetMenu(menuName = "Towers/Paint Bucket")]
    public class PaintBucketData : TowerData
    {
        [SerializeField] private Color[] m_paintColors;
        public Color[] m_PaintColors => m_paintColors;
    }
}