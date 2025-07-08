using System;
using Code.GamePlay.InputHandler;
using Code.GamePlay.PlayerBall;
using Code.GamePlay.TargetOnLevel;
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
        private const float RadiusToFindDoor = 100f;

        [SerializeField] private float _minBallScale = 0.2f;
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
        private Transform _doorTransform;
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

            if (_playerBall == null)
                throw new NullReferenceException("Player ball is null");

            _ball = _playerBall.GetComponent<Ball>();
            
            if (_ball == null)
                throw new NullReferenceException("Ball component not found");

            _roadTransform = _roadProvider.Instance.transform;
            
            if (_roadTransform == null)
                throw new NullReferenceException("Road transform is null");

            _initialBallScale = _playerBall.transform.localScale;
            _initialRoadWidth = _roadTransform.localScale.z;

            _pathCleared = false;
            _bulletAlive = false;

            _tapInputHandler.TapStarted += OnTapStarted;
            _tapInputHandler.TapEnded += OnTapEnded;

            TryFindDoor();
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

            if (newScale.x <= _initialBallScale.x * _minBallScale)
            {
                newScale = _initialBallScale * _minBallScale;
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
                    _ball.JumpTo(_levelTargetProvider.Instance.transform.position);
            }
        }

        private bool IsPathBlockedByObstacle()
        {
            if (_doorTransform == null)
                return true;

            Vector3 start = _playerBall.transform.position;
            Vector3 target = _levelTargetProvider.Instance != null
                ? _levelTargetProvider.Instance.transform.position
                : _doorTransform.position;

            float distance = Vector3.Distance(start, target);
            float radius = distance * 1.5f;

            Collider[] colliders = Physics.OverlapSphere(start, radius);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Obstacle>(out var obstacle) && !obstacle.IsInfected)
                    return true;
            }

            return false;
        }

        private void TryFindDoor()
        {
            Collider[] colliders = Physics.OverlapSphere(_playerBall.transform.position, RadiusToFindDoor);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Door door))
                {
                    _doorTransform = door.transform;
                    return;
                }
            }

            throw new Exception("Door not found in radius");
        }

        private Bullet CreateBullet()
        {
            Vector3 spawnPoint = _playerBall.transform.position
                                 + _playerBall.transform.right * -1.8f
                                 + Vector3.up * 0.1f;

            GameObject bulletGO = _bulletFactory.CreateBullet(spawnPoint);
            return bulletGO.GetComponent<Bullet>();
        }

        private void UpdateRoadScale(float currentBallScaleX)
        {
            float scaleRatio = currentBallScaleX / _initialBallScale.x;
            Vector3 newRoadScale = _roadTransform.localScale;
            newRoadScale.z = _initialRoadWidth * scaleRatio;
            _roadTransform.localScale = newRoadScale;
        }

        private void GameOver()
        {
            Debug.LogError("Game Over");
            // TODO: Notify game manager / UI
        }
    }
}