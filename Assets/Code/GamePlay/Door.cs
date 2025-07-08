using Code.GamePlay.PlayerBall;
using DG.Tweening;
using UnityEngine;

namespace Code.GamePlay
{
    public class Door : MonoBehaviour
    {
        private const float RadiusToOpen = 5f;
        private const float OpenHeight = 10f;
        private const float OpenDuration = 1f;

        private bool _isOpened;

        private void Update()
        {
            if (_isOpened)
                return;

            TryFindBall();
        }

        private void TryFindBall()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, RadiusToOpen);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Ball _) == false)
                    continue;

                Open();
                break;
            }
        }

        private void Open()
        {
            _isOpened = true;

            transform.DOMoveY(transform.position.y + OpenHeight, OpenDuration)
                .SetEase(Ease.OutCubic);
        }
    }
}