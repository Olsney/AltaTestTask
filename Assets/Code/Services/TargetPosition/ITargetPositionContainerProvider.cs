using Code.GamePlay.TargetOnLevel;

namespace Code.Services.TargetPosition
{
    public interface ITargetPositionContainerProvider
    {
        void SetContainer(LevelTargetPositionContainer container);
        LevelTargetPositionContainer GetContainer();
    }
}