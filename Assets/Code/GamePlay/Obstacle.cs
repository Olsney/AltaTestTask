using UnityEngine;

namespace Code.GamePlay
{
    public class Obstacle : MonoBehaviour
    {
        private const float DelayBeforeDestroy = 1f;

        private readonly Color InfectedColor = Color.yellow;
        
        public bool IsInfected { get; private set; }

        public void Infect()
        {
            if (IsInfected)
                return;

            IsInfected = true;
            
            Renderer renderer = GetComponent<Renderer>();

            if (renderer == null)
                return;
            
            renderer.material = new Material(renderer.material);
            renderer.material.color = InfectedColor;
            
            Collider collider = GetComponent<Collider>();
            collider.enabled = false;

            Destroy(gameObject, DelayBeforeDestroy);
        }
    }
}