using Code.Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Factory.Armament
{
    public class BulletFactory : IBulletFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assets;

        public BulletFactory(IInstantiator instantiator, IAssetProvider assets)
        {
            _instantiator = instantiator;
            _assets = assets;
        }
        
        public GameObject CreateBullet(Vector3 at)
        {
            GameObject prefab = _assets.Load(AssetPath.PlayerBulletPath);
            
            return _instantiator.InstantiatePrefab(prefab, at, Quaternion.identity, null);
        }
    }
}