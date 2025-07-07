using Code.Infrastructure.States;

namespace Code.Infrastructure.Factory
{
    public interface IStateFactory
    {
        T GetState<T>() where T : class, IExitableState;
    }
}