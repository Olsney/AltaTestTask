using System;
using Code.Services.Inputs;
using UnityEngine;
using Zenject;

namespace Code.GamePlay
{
    public class TapInputHandler : MonoBehaviour
    {
        public event Action TapStarted;
        public event Action TapEnded;
        
        private IInputService _inputService;
        private bool _isHeld;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        private void Update()
        {
            if (_inputService.IsTapStarted())
            {
                TapStarted?.Invoke();
                _isHeld = true;
            }

            if (_inputService.IsTapEnded() && _isHeld)
            {
                TapEnded?.Invoke();
                _isHeld = false;
            }
        }
    }
}