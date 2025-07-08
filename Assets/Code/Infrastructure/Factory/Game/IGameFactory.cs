using UnityEngine;

namespace Code.Infrastructure.Factory
{
    public interface IGameFactory
    {
        GameObject CreatePlayerBall();
        GameObject CreateTapInputHandler();
        GameObject CreateScaler();
        GameObject CreateLevelTarget();
    }
}