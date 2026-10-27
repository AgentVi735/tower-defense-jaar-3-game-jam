using UnityEngine;

namespace FckAI.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        private EnemyData m_data;
        
        public void Initialise(EnemyData data)
        {
            m_data = data;
            gameObject.SetActive(false);
        }
    }
}