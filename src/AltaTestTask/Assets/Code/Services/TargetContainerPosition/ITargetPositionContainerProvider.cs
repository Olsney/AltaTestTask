using Code.GamePlay.TargetOnLevel;

namespace Code.Services.TargetContainerPosition
{
    public interface ITargetPositionContainerProvider
    {
        void SetContainer(LevelTargetPositionContainer container);
        LevelTargetPositionContainer GetContainer();
    }
}