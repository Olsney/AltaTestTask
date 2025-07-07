using UnityEngine;

namespace Code.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Load(string path) => 
            Resources.Load<GameObject>(path);
    }
}