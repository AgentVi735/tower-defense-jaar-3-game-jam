using UnityEngine;

namespace FckAI.Towers
{
    public class Tower : MonoBehaviour
    {
        private TowerData m_data;

        public virtual void Initialise(TowerData data)
        {
            m_data = data;
        }
    }
}