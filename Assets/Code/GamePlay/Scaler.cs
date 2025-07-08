using Code.Infrastructure.Factory.Armament;
using Code.Services.PlayerBallProvider;
using Code.Services.TapInputHandlerProvider;
using UnityEngine;
using Zenject;

namespace Code.GamePlay
{
    public class Scaler : MonoBehaviour
    {
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

        [Inject]
        public void Construct(ITapInputHandlerProvider tapInputHandlerProvider,
            IPlayerBallProvider playerBallProvider,
            IBulletFactory bulletFactory)
        {
            _tapInputHandlerProvider = tapInputHandlerProvider;
            _playerBallProvider = playerBallProvider;
            _bulletFactory = bulletFactory;
        }

        public void Initialize()
        {
            _tapInputHandler = _tapInputHandlerProvider.GetTapInputHandler();
            _playerBall = _playerBallProvider.GetBall();
            _initialBallScale = _playerBall.transform.localScale;

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
        }

        private Bullet CreateBullet()
        {
            Vector3 spawnPoint = _playerBall.transform.position 
                                 + _playerBall.transform.right * - 1.8f 
                                 + Vector3.up * 0.1f;

            GameObject bullet = _bulletFactory.CreateBullet(spawnPoint);
            return bullet.GetComponent<Bullet>();
        }

        private void GameOver()
        {
            Debug.LogError("Game Over — Ball scale reached minimum!");
        }
    }
}
