using Code.GamePlay;
using Code.Infrastructure.AssetManagement;
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

        public GameFactory(IInstantiator instantiator, 
            IAssetProvider assets,
            ITapInputHandlerProvider tapInputHandlerProvider,
            IPlayerBallProvider playerBallProvider)
        {
            _instantiator = instantiator;
            _assets = assets;
            _tapInputHandlerProvider = tapInputHandlerProvider;
            _playerBallProvider = playerBallProvider;
        }
        
        public GameObject CreatePlayerBall(Vector3 at)
        {
            GameObject prefab = _assets.Load(AssetPath.PlayerBallPath);

            GameObject instance = _instantiator.InstantiatePrefab(prefab, at, Quaternion.identity, null);
            
            _playerBallProvider.SetBall(instance);

            return instance;
        }

        public GameObject CreateTapInputHandler()
        {
            GameObject prefab = _assets.Load(AssetPath.TapInputHandlerPath);

            GameObject instance = _instantiator.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, null);
            TapInputHandler tapInputHandlerProvider = instance.GetComponent<TapInputHandler>();
            
            _tapInputHandlerProvider.SetInputHandler(tapInputHandlerProvider);
            
            return instance;
        }
    }
}