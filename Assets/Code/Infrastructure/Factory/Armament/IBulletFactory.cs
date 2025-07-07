using UnityEngine;

namespace Code.Infrastructure.Factory.Armament
{
    public interface IBulletFactory
    {
        GameObject CreateBullet(Vector3 at);
    }
}