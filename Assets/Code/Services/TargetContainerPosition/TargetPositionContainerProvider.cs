using Code.GamePlay.TargetOnLevel;

namespace Code.Services.TargetContainerPosition
{
    public class TargetPositionContainerProvider : ITargetPositionContainerProvider
    {
        private LevelTargetPositionContainer _container;

        public void SetContainer(LevelTargetPositionContainer container) =>
            _container = container;

        public LevelTargetPositionContainer GetContainer() =>
            _container;
    }
}