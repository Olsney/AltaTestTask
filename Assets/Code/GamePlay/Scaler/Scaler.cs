using System;
using Code.GamePlay.InputHandler;
using Code.GamePlay.TargetOnLevel;
using Code.Infrastructure.Factory.Armament;
using Code.Services.PlayerBallProvider;
using Code.Services.TapInputHandlerProvider;
using Code.Services.TargetProvider;
using UnityEngine;
using Zenject;

namespace Code.GamePlay.Scaler
{
    public class Scaler : MonoBehaviour
    {
        private const int MaxShots = 7;
        private const float CheckDelay = 2f;
        private const float RadiusToFindDoor = 100f;

        [SerializeField] private float _minBallScale = 0.2f;
        [SerializeField] private float _scaleDecreaseSpeed = 0.25f;
        [SerializeField] private float _infectionRadiusPerMoment = 2.0f;
        [SerializeField] private float _bulletScaleModifier = 0.5f;
        [SerializeField] private float _minInfectionRadius = 1.0f;

        private ITapInputHandlerProvider _tapInputHandlerProvider;
        private IBulletFactory _bulletFactory;
        private IPlayerBallProvider _playerBallProvider;

        private TapInputHandler _tapInputHandler;
        private GameObject _playerBall;
        private Bullet _bullet;
        private Transform _bulletTransform;
        private Vector3 _initialBallScale;

        private bool _isCharging;
        private float _infectionRadius;
        
        private int _shotsFired;
        private Transform _doorTransform;
        private ILevelTargetProvider _levelTargetProvider;

        [Inject]
        public void Construct(ITapInputHandlerProvider tapInputHandlerProvider,
            IPlayerBallProvider playerBallProvider,
            IBulletFactory bulletFactory,
            ILevelTargetProvider levelTargetProvider)
        {
            _tapInputHandlerProvider = tapInputHandlerProvider;
            _playerBallProvider = playerBallProvider;
            _bulletFactory = bulletFactory;
            _levelTargetProvider = levelTargetProvider;
        }

        public void Initialize()
        {
            _tapInputHandler = _tapInputHandlerProvider.GetTapInputHandler();
            _playerBall = _playerBallProvider.GetBall();
            _initialBallScale = _playerBall.transform.localScale;
            LevelTarget levelTarget = _levelTargetProvider.Instance;

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
                OnTapEnded();
                GameOver();
                
                return;
            }

            _playerBall.transform.localScale = newScale;
            _infectionRadius += delta * _infectionRadiusPerMoment;
            _bulletTransform.localScale += new Vector3(delta, delta, delta);
        }

        private void OnTapStarted()
        {
            if (_shotsFired >= MaxShots)
            {
                Debug.Log("No shots remaining");
                return;
            }

            
            _isCharging = true;
            _infectionRadius = 0f;

            _bullet = CreateBullet();
            _bulletTransform = _bullet.transform;

            _bulletTransform.localScale = Vector3.one * _bulletScaleModifier;
        }

        private void OnTapEnded()
        {
            if (!_isCharging)
                return;

            _isCharging = false;

            float finalInfectionRadius = Mathf.Max(_infectionRadius, _minInfectionRadius);
            Vector3 direction = Vector3.left;

            _bullet.Initialize(direction, finalInfectionRadius);
            
            _shotsFired++;
            if (_shotsFired >= MaxShots)
                StartCoroutine(CheckPathAndGameOver());
        }

        private void TryFindDoor()
        {
            Collider[] colliders = Physics.OverlapSphere(_playerBall.transform.position, RadiusToFindDoor);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Door door) == false)
                    continue;
                
                _doorTransform = door.transform;
            }
            
            if(_doorTransform == null)
                throw new Exception("Door is not found");
        }

        private Bullet CreateBullet()
        {
            Vector3 spawnPoint = _playerBall.transform.position 
                                 + _playerBall.transform.right * - 1.8f 
                                 + Vector3.up * 0.1f;

            GameObject bullet = _bulletFactory.CreateBullet(spawnPoint);
            return bullet.GetComponent<Bullet>();
        }
        
        private System.Collections.IEnumerator CheckPathAndGameOver()
        {
            yield return new WaitForSeconds(CheckDelay);

            if (IsPathBlocked())
                GameOver();
        }

        private bool IsPathBlocked()
        {
            if (_doorTransform == null)
                return false;

            Vector3 start = _playerBall.transform.position;
            Vector3 direction = _doorTransform.position - start;
            
            float distance = direction.magnitude;

            RaycastHit[] hits = Physics.RaycastAll(start, direction.normalized, distance);
            
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<Obstacle>() != null)
                    return true;
            }

            return false;
        }


        private void GameOver()
        {
            Debug.LogError("Game Over — Ball scale reached minimum!");
        }
    }
}
