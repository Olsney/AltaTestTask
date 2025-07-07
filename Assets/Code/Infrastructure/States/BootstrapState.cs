using Code.Services.StaticData;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataService _staticData;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, IStaticDataService staticData)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _staticData = staticData;
        }

        public void Enter()
        {
            if (SceneManager.GetActiveScene().name == Initial)
            {
                EnterLoadLevel();
            }
            else
            {
                _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
            }
        }

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadProgressState>();

        public void Exit()
        {
        }
    }
}