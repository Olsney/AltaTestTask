using UnityEngine;

namespace Code.Infrastructure.Factory
{
    public interface IGameFactory
    {
        GameObject CreatePlayerBall(Vector3 at);
        GameObject CreateTapInputHandler();
    }
}