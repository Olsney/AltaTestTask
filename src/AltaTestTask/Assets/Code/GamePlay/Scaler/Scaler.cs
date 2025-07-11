using System;
using Code.GamePlay.InputHandler;
using Code.GamePlay.PlayerBall;
using Code.Infrastructure.Factory.Armament;
using Code.Services.PlayerBallProvider;
using Code.Services.Road;
using Code.Services.TapInputHandlerProvider;
using Code.Services.TargetProvider;
using UnityEngine;
using Zenject;

namespace Code.GamePlay.Scaler
{
    public class Scaler : MonoBehaviour
    {
        private const int MaxShots = 5;
        private const float MinPercentOfRestScale = 0.2f;

        [SerializeField] private float _scaleDecreaseSpeed = 0.25f;
        [SerializeField] private float _infectionRadiusPerMoment = 2.0f;
        [SerializeField] private float _bulletScaleModifier = 0.5f;
        [SerializeField] private float _minInfectionRadius = 1.0f;

        private ITapInputHandlerProvider _tapInputHandlerProvider;
        private IBulletFactory _bulletFactory;
        private IPlayerBallProvider _playerBallProvider;
        private ILevelTargetProvider _levelTargetProvider;
        private IRoadProvider _roadProvider;

        private TapInputHandler _tapInputHandler;
        private GameObject _playerBall;
        private Ball _ball;
        private Bullet _bullet;
        private Transform _bulletTransform;
        private Transform _roadTransform;

        private Vector3 _initialBallScale;
        private float _initialRoadWidth;

        private bool _isCharging;
        private bool _pathCleared;
        private bool _bulletAlive;
        private float _infectionRadius;
        private int _shotsFired;

        [Inject]
        public void Construct(
            ITapInputHandlerProvider tapInputHandlerProvider,
            IPlayerBallProvider playerBallProvider,
            IBulletFactory bulletFactory,
            ILevelTargetProvider levelTargetProvider,
            IRoadProvider roadProvider)
        {
            _tapInputHandlerProvider = tapInputHandlerProvider;
            _playerBallProvider = playerBallProvider;
            _bulletFactory = bulletFactory;
            _levelTargetProvider = levelTargetProvider;
            _roadProvider = roadProvider;
        }

        public void Initialize()
        {
            _tapInputHandler = _tapInputHandlerProvider.GetTapInputHandler();
            _playerBall = _playerBallProvider.GetBall();

            _ball = _playerBall.GetComponent<Ball>();

            _roadTransform = _roadProvider.Instance.transform;

            _initialBallScale = _playerBall.transform.localScale;
            _initialRoadWidth = _roadTransform.localScale.z;

            _pathCleared = false;
            _bulletAlive = false;

            _tapInputHandler.TapStarted += OnTapStarted;
            _tapInputHandler.TapEnded += OnTapEnded;
        }

        private void OnDestroy()
        {
            if (_tapInputHandler != null)
            {
                _tapInputHandler.TapStarted -= OnTapStarted;
                _tapInputHandler.TapEnded -= OnTapEnded;
            }

            if (_bullet != null)
                _bullet.BulletDestroyed -= OnBulletDestroyed;
        }

        private void Update()
        {
            if (!_isCharging)
                return;

            float delta = _scaleDecreaseSpeed * Time.deltaTime;
            Vector3 newScale = _playerBall.transform.localScale - new Vector3(delta, delta, delta);

            if (newScale.x <= _initialBallScale.x * MinPercentOfRestScale)
            {
                newScale = _initialBallScale * MinPercentOfRestScale;
                _playerBall.transform.localScale = newScale;
                _infectionRadius += delta * _infectionRadiusPerMoment;

                UpdateRoadScale(newScale.x);
                _isCharging = false;
                OnTapEnded();
                GameOver();
                
                return;
            }

            _playerBall.transform.localScale = newScale;
            _infectionRadius += delta * _infectionRadiusPerMoment;

            if (_bulletTransform != null)
                _bulletTransform.localScale += new Vector3(delta, delta, delta);

            UpdateRoadScale(newScale.x);
        }

        private void OnTapStarted()
        {
            if (_pathCleared || _shotsFired >= MaxShots || _bulletAlive)
                return;

            _isCharging = true;
            _infectionRadius = 0f;

            _bullet = CreateBullet();
            _bulletTransform = _bullet.transform;
            _bulletTransform.localScale = Vector3.one * _bulletScaleModifier;

            _bullet.BulletDestroyed += OnBulletDestroyed;
            _bulletAlive = true;
        }

        private void OnTapEnded()
        {
            if (!_isCharging || _pathCleared || _bullet == null)
                return;

            _isCharging = false;

            float finalInfectionRadius = Mathf.Max(_infectionRadius, _minInfectionRadius);
            Vector3 direction = Vector3.left;

            _bullet.Initialize(direction, finalInfectionRadius);
            _shotsFired++;
        }

        private void OnBulletDestroyed(Bullet bullet)
        {
            _bulletAlive = false;
            bullet.BulletDestroyed -= OnBulletDestroyed;

            if (IsPathBlockedByObstacle())
            {
                if (_shotsFired >= MaxShots)
                    GameOver();
            }
            else
            {
                _pathCleared = true;

                _tapInputHandler.TapStarted -= OnTapStarted;
                _tapInputHandler.TapEnded -= OnTapEnded;

                if (_ball != null && _levelTargetProvider.Instance != null)
                {
                    Vector3 target = _levelTargetProvider.Instance.transform.position;
                    _ball.JumpTo(target);
                }
            }
        }

        private bool IsPathBlockedByObstacle()
        {
            if (_roadTransform == null)
                return true;

            Vector3 roadPosition = _roadTransform.position;
            Vector3 roadScale = _roadTransform.localScale;

            Vector3 center = roadPosition + Vector3.up * 1.0f;

            float halfLength = roadScale.x * 5f;
            float halfWidth = roadScale.z / 2f;
            float halfHeight = 1.5f;

            Vector3 halfExtents = new Vector3(halfLength, halfHeight, halfWidth);

            Collider[] colliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Obstacle>(out var obstacle) && !obstacle.IsInfected)
                    return true;
            }

            return false;
        }

        private Bullet CreateBullet()
        {
            Vector3 spawnPoint = _playerBall.transform.position
                                 + _playerBall.transform.right * -1.8f
                                 + Vector3.up * 0.1f;

            GameObject bullet = _bulletFactory.CreateBullet(spawnPoint);
            return bullet.GetComponent<Bullet>();
        }

        private void UpdateRoadScale(float currentBallScaleX)
        {
            float scaleRatio = currentBallScaleX / _initialBallScale.x;
            Vector3 newRoadScale = _roadTransform.localScale;
            newRoadScale.z = _initialRoadWidth * scaleRatio;
            _roadTransform.localScale = newRoadScale;
        }

        private void GameOver() => 
            Debug.Log("Game Over");
    }
}