using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Factory;
using Code.Infrastructure.Factory.Armament;
using Code.Infrastructure.States;
using Code.Services.Inputs;
using Code.Services.PlayerBallProvider;
using Code.Services.StaticData;
using Code.Services.TapInputHandlerProvider;
using Code.Services.TargetPosition;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            BindCoroutine();
            BindFactories();
            BindStates();
            BindServices();
            BindSceneLoader();
        }

        private void BindCoroutine() => 
            Container.Bind<ICoroutineRunner>().FromInstance(this).AsSingle();

        private void BindFactories()
        {
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<IStateFactory>().To<StateFactory>().AsSingle();
            Container.Bind<IBulletFactory>().To<BulletFactory>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
        }
        
        private void BindServices()
        {
            BindInputService();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<ITapInputHandlerProvider>().To<TapInputHandlerProvider>().AsSingle();
            Container.Bind<IPlayerBallProvider>().To<PlayerBallProvider>().AsSingle();
            Container.Bind<ITargetPositionContainerProvider>().To<TargetPositionContainerProvider>().AsSingle();
        }
        
        private void BindInputService()
        {
            if (Application.isEditor)
                Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
            else
                Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
        }
        
        private void BindSceneLoader() => 
            Container.Bind<SceneLoader>().AsSingle();
    }
}
