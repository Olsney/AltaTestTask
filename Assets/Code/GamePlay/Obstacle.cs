using UnityEngine;

namespace Code.GamePlay
{
    public class Obstacle : MonoBehaviour
    {
        private const float DelayBeforeDestroy = 2f;
        
        private readonly Color InfectedColor = Color.yellow;

        public void Infect()
        {
            Renderer renderer = GetComponent<Renderer>();
            
            if (renderer != null)
            {
                renderer.material = new Material(renderer.material);
                renderer.material.color = InfectedColor;
            }
            else
            {
                Debug.Log("Obstacle renderer is null");
            }

            Destroy(gameObject, DelayBeforeDestroy);
        }
    }
}