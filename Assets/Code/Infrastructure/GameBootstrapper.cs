using Code.Infrastructure.Factory;
using Code.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private GameStateMachine _gameStateMachine;
        private IGameFactory _gameFactory;
        
        [Inject]
        private void Construct(GameStateMachine gameStateMachine, IGameFactory gameFactory)
        {
            _gameStateMachine = gameStateMachine;
            _gameFactory = gameFactory;
        }
        
        private void Start()
        {
            _gameStateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}