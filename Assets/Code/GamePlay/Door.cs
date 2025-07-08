using System;
using Code.GamePlay.PlayerBall;
using UnityEngine;

namespace Code.GamePlay
{
    public class Door : MonoBehaviour
    {
        private const float RadiusToOpen = 5f;

        private void Update()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, RadiusToOpen);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Ball ball) == false)
                    continue;

                Open();
            }
        }

        private void Open()
        {
            
        }
    }
}