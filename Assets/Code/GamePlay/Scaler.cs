using Code.Infrastructure.Factory.Armament;
using Code.Services.PlayerBallProvider;
using Code.Services.TapInputHandlerProvider;
using UnityEngine;
using Zenject;

namespace Code.GamePlay
{
    public class Scaler : MonoBehaviour
    {
        private ITapInputHandlerProvider _tapInputHandlerProvider;
        private IBulletFactory _bulletFactory;
        private IPlayerBallProvider _playerBallProvider;
        
        private TapInputHandler _tapInputHandler;
        private GameObject _playerBall;
        private float _currentSize;

        [Inject]
        public void Construct(ITapInputHandlerProvider tapInputHandlerProvider,
            IPlayerBallProvider playerBallProvider,
            IBulletFactory bulletFactory)
        {
            _tapInputHandlerProvider = tapInputHandlerProvider;
            _playerBallProvider = playerBallProvider;
            _bulletFactory = bulletFactory;
        }

        private void Awake()
        {
            _tapInputHandler = _tapInputHandlerProvider.GetTapInputHandler();
            _playerBall = _playerBallProvider.GetBall();
            _currentSize = _playerBall.transform.localScale.x;
            
            _tapInputHandler.TapStarted += OnTapStarted;
            _tapInputHandler.TapEnded += OnTapEnded;
        }

        private void OnDestroy()
        {
            _tapInputHandler.TapStarted -= OnTapStarted;
            _tapInputHandler.TapEnded -= OnTapEnded;
        }

        private void OnTapStarted()
        {
            
        }

        private void OnTapEnded()
        {
        }
    }
}