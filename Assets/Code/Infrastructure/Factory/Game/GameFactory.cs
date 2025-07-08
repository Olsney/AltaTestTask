using Code.GamePlay;
using Code.GamePlay.InputHandler;
using Code.GamePlay.Scaler;
using Code.Infrastructure.AssetManagement;
using Code.Services.Inputs;
using Code.Services.PlayerBallProvider;
using Code.Services.TapInputHandlerProvider;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assets;
        private readonly ITapInputHandlerProvider _tapInputHandlerProvider;
        private readonly IPlayerBallProvider _playerBallProvider;
        private readonly IInputService _inputService;

        public GameFactory(IInstantiator instantiator, 
            IAssetProvider assets,
            ITapInputHandlerProvider tapInputHandlerProvider,
            IPlayerBallProvider playerBallProvider, 
            IInputService inputService)
        {
            _instantiator = instantiator;
            _assets = assets;
            _tapInputHandlerProvider = tapInputHandlerProvider;
            _playerBallProvider = playerBallProvider;
            _inputService = inputService;
        }
        
        public GameObject CreatePlayerBall()
        {
            GameObject prefab = _assets.Load(AssetPath.PlayerBallPath);

            GameObject instance = _instantiator.InstantiatePrefab(prefab);
            
            _playerBallProvider.SetBall(instance);

            return instance;
        }

        public GameObject CreateTapInputHandler()
        {
            GameObject prefab = _assets.Load(AssetPath.TapInputHandlerPath);

            GameObject instance = _instantiator.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, null);
            TapInputHandler tapInputHandler = instance.GetComponent<TapInputHandler>();
            
            _tapInputHandlerProvider.SetInputHandler(tapInputHandler);
            
            return instance;
        }
        
        public GameObject CreateScaler()
        {
            GameObject prefab = _assets.Load(AssetPath.ScalerPath);

            GameObject instance = _instantiator.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, null);
            Scaler scaler = instance.GetComponent<Scaler>();

            scaler.Initialize();
            
            return instance;
        }
    }
}