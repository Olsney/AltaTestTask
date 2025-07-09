using UnityEngine;

namespace Code.Services.PlayerBallProvider
{
    public class PlayerBallProvider : IPlayerBallProvider
    {
        private GameObject _playerBallPrefab;

        public void SetBall(GameObject playerBallPrefab) =>
            _playerBallPrefab = playerBallPrefab;

        public GameObject GetBall() =>
            _playerBallPrefab;
    }
}