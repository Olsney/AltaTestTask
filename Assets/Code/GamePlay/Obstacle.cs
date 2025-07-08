using UnityEngine;

namespace Code.GamePlay
{
    public class Obstacle : MonoBehaviour
    {
        private const float DelayBeforeDestroy = 1f;

        private readonly Color InfectedColor = Color.yellow;

        public void Infect()
        {
            Renderer renderer = GetComponent<Renderer>();

            if (renderer == null)
                return;
            
            renderer.material = new Material(renderer.material);
            renderer.material.color = InfectedColor;

            Destroy(gameObject, DelayBeforeDestroy);
        }
    }
}