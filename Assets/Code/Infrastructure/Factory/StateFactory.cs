using Code.Infrastructure.States;
using Zenject;

namespace Code.Infrastructure.Factory
{
    public class StateFactory : IStateFactory
    {
        private readonly DiContainer _container;

        public StateFactory(DiContainer container)
        {
            _container = container;
        }

        public T GetState<T>() where T : class, IExitableState =>
            _container.Resolve<T>();
    }
}