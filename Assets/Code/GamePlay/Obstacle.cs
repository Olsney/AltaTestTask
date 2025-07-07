using UnityEngine;

namespace Code.GamePlay
{
    public class Obstacle : MonoBehaviour
    {
        private const float DelayBeforeDestroy = 5f;

        public void Infect()
        {
            Destroy(gameObject, DelayBeforeDestroy);
        }
    }
}