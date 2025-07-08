using Code.Infrastructure.Factory;
using Code.Services.StaticData;
using UnityEngine;

namespace Code.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialBallPoint";
        private const string EmptySceneName = "Empty";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameFactory _gameFactory;

        public LoadLevelState(
            GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            IGameFactory gameFactory,
            IStaticDataService staticDataService 
            )
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _staticDataService = staticDataService;
        }

        public void Enter(string sceneName)
        {
            _gameFactory.CreatePlayerBall();
            
            _sceneLoader.Load(EmptySceneName, () =>  _sceneLoader.Load(sceneName, OnLoaded));
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            InitGameWorld();

            _stateMachine.Enter<GameLoopState>();
        }

        private void LoadRequstedScene(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        private void InitGameWorld()
        {
            GameObject tapInputHandler = _gameFactory.CreateTapInputHandler();
            GameObject scaler = _gameFactory.CreateScaler();
            _gameFactory.CreateLevelTarget();
        }
    }
}