using System.Collections;
using UnityEngine;

namespace Code.GamePlay.PlayerBall
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight = 0.5f;
        [SerializeField] private float _jumpDuration = 0.015f;
        [SerializeField] private float _stepDistance = 3.5f;
        [SerializeField] private float _minDistanceToTarget = 0.2f;
        [SerializeField] private float _groundOffset = 0.05f;

        private Coroutine _jumpCoroutine;

        public void JumpTo(Vector3 finalTarget)
        {
            if (_jumpCoroutine != null)
                StopCoroutine(_jumpCoroutine);

            _jumpCoroutine = StartCoroutine(JumpSequence(finalTarget));
        }

        private IEnumerator JumpSequence(Vector3 finalTarget)
        {
            Vector3 flatFinal = new Vector3(finalTarget.x, 0f, finalTarget.z);

            while (true)
            {
                Vector3 current = transform.position;
                Vector3 flatCurrent = new Vector3(current.x, 0f, current.z);
                float distance = Vector3.Distance(flatCurrent, flatFinal);

                if (distance <= _minDistanceToTarget)
                    break;

                Vector3 direction = (flatFinal - flatCurrent).normalized;
                float step = Mathf.Min(_stepDistance, distance);

                Vector3 nextTarget = flatCurrent + direction * step;
                nextTarget.y = _groundOffset;

                yield return StartCoroutine(SingleJumpRoutine(current, nextTarget));
            }

            Vector3 finalLanding = finalTarget;
            finalLanding.y = _groundOffset;
            transform.position = finalLanding;
        }

        private IEnumerator SingleJumpRoutine(Vector3 start, Vector3 end)
        {
            float elapsed = 0f;

            while (elapsed < _jumpDuration)
            {
                float t = elapsed / _jumpDuration;
                float height = Mathf.Sin(Mathf.PI * t) * _jumpHeight;
                Vector3 flatPos = Vector3.Lerp(start, end, t);
                transform.position = flatPos + Vector3.up * height;
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
        }
    }
}
