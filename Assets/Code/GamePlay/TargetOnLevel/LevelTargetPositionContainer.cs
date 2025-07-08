using Code.Services.TargetPosition;
using UnityEngine;
using Zenject;

namespace Code.GamePlay.TargetOnLevel
{
    public class LevelTargetPositionContainer : MonoBehaviour
    {
        private ITargetPositionContainerProvider _container;

        [Inject]
        public void Construct(ITargetPositionContainerProvider container)
        {
            _container = container;
        }

        public void Awake()
        {
            _container.SetContainer(this);
        }
    }
}