using System.Collections;
using UnityEngine;

namespace Code.GamePlay.PlayerBall
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight = 1f;
        [SerializeField] private float _jumpDuration = 1f;

        private Coroutine _jumpCoroutine;

        public void JumpTo(Vector3 target)
        {
            if (_jumpCoroutine != null)
                StopCoroutine(_jumpCoroutine);

            _jumpCoroutine = StartCoroutine(JumpRoutine(target));
        }

        private IEnumerator JumpRoutine(Vector3 target)
        {
            Vector3 start = transform.position;
            float elapsed = 0f;

            while (elapsed < _jumpDuration)
            {
                float timeResult = elapsed / _jumpDuration;
                float height = Mathf.Sin(Mathf.PI * timeResult) * _jumpHeight;
                transform.position = Vector3.Lerp(start, target, timeResult) + Vector3.up * height;
                elapsed += Time.deltaTime;
                
                yield return null;
            }

            transform.position = target;
        }
    }
}