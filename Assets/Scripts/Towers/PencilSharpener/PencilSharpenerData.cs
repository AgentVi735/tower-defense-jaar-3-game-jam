using UnityEngine;

namespace FckAI.Towers.PencilSharpener
{
    [CreateAssetMenu(menuName = "Towers/Pencil Sharpener")]
    public class PencilSharpenerData : TowerData
    {
        [SerializeField] private PencilProjectile m_pencilProjectilePrefab;
        public PencilProjectile m_PencilProjectilePrefab => m_pencilProjectilePrefab;
        [SerializeField] private int m_maxPencilProjectiles;
        public int m_MaxPencilProjectiles => m_maxPencilProjectiles;
    }
}