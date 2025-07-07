using UnityEngine;

namespace Code.Infrastructure.AssetManagement
{
    public interface IAssetProvider 
    {
        GameObject Load(string path);

    }
}