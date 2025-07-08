using System;
using Code.Services.PlayerBallProvider;
using UnityEngine;
using Zenject;

namespace Code.GamePlay
{
    public class PlayerBallInitialPoint : MonoBehaviour
    {
        private IPlayerBallProvider _ballProvider;

        [Inject]
        public void Construct(IPlayerBallProvider ballProvider)
        {
            _ballProvider = ballProvider;
        }

        private void Start()
        {
            GameObject ball = _ballProvider.GetBall();
            
            ball.transform.position = transform.position;
        }
    }
}