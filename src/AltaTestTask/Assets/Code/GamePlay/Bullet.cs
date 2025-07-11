using System;
using UnityEngine;

namespace Code.GamePlay
{
    public class Bullet : MonoBehaviour
    {
        private const float DelayBeforeDestroy = 0.5f;

        [SerializeField] private float _speed = 10f;

        private Vector3 _direction;
        private bool _canMove;
        private float _infectionRadius;

        public event Action<Bullet> BulletDestroyed;

        public void Initialize(Vector3 direction, float infectionRadius)
        {
            _direction = direction.normalized;
            _infectionRadius = infectionRadius;
            _canMove = true;
        }

        private void Update()
        {
            if (_canMove)
                transform.position += _direction * (_speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Obstacle>(out var obstacle))
                return;

            _canMove = false;
            Explode();
            Invoke(nameof(DestroySelf), DelayBeforeDestroy);
        }

        private void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _infectionRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Obstacle>(out var obstacle)) 
                    obstacle.Infect();
            }
        }

        private void DestroySelf()
        {
            BulletDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _infectionRadius);
        }
#endif
    }
}