namespace Code.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string MainSceneName = "Main";
        
        private readonly GameStateMachine _gameStateMachine;

        public LoadProgressState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(MainSceneName);
        }

        public void Exit()
        {
        }
    }
}