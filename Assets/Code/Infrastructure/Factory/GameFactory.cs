using Code.Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assets;

        public GameFactory(IInstantiator instantiator, IAssetProvider assets)
        {
            _instantiator = instantiator;
            _assets = assets;
        }
        
        public GameObject CreatePlayerBall(Vector3 at)
        {
            GameObject prefab = _assets.Load(AssetPath.PlayerBallPath);
            
            return _instantiator.InstantiatePrefab(prefab, at, Quaternion.identity, null);
        }
    }
}