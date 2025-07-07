using UnityEngine;

namespace Code.Services.Inputs
{
    public class MobileInputService : IInputService
    {
        public bool IsTapStarted()
        {
            return Input.touchCount > 0 &&
                   Input.GetTouch(0).phase == TouchPhase.Began;
        }

        public bool IsTapHeld()
        {
            if (Input.touchCount == 0)
                return false;

            TouchPhase phase = Input.GetTouch(0).phase;
            
            return phase == TouchPhase.Stationary || phase == TouchPhase.Moved;
        }

        public bool IsTapEnded()
        {
            return Input.touchCount > 0 &&
                   Input.GetTouch(0).phase == TouchPhase.Ended;
        }
    }
}