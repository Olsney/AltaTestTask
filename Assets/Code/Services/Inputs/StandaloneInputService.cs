using UnityEngine;

namespace Code.Services.Inputs
{
    public class StandaloneInputService : IInputService
    {
        public bool IsTapStarted() => 
            Input.GetMouseButtonDown(0);

        public bool IsTapHeld() => 
            Input.GetMouseButton(0);

        public bool IsTapEnded() => 
            Input.GetMouseButtonUp(0);
    }
}