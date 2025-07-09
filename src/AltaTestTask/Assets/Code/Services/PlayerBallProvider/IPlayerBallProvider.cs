using UnityEngine;

namespace Code.Services.PlayerBallProvider
{
    public interface IPlayerBallProvider
    {
        void SetBall(GameObject playerBallPrefab);
        GameObject GetBall();
    }
}