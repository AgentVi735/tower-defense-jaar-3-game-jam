using UnityEngine;

namespace FckAI.Towers.PaintBucket
{
    [CreateAssetMenu(menuName = "Towers/Paint Bucket")]
    public class PaintBucketData : TowerData
    {
        [SerializeField] private Color[] m_paintColors;
        public Color[] m_PaintColors => m_paintColors;
        [SerializeField] private PaintProjectile m_paintProjectilePrefab;
        public PaintProjectile m_PaintProjectilePrefab => m_paintProjectilePrefab;
        [SerializeField] private PaintArea m_paintAreaPrefab;
        public PaintArea m_PaintAreaPrefab => m_paintAreaPrefab;
        [SerializeField] private int m_maxPaintAreas;
        public int m_MaxPaintAreas => m_maxPaintAreas;
        [SerializeField] private float m_paintAreaTime;
        public float m_PaintAreaTime => m_paintAreaTime;
        [SerializeField] private float m_paintHitTick;
        public float m_PaintHitTick => m_paintHitTick;
    }
}