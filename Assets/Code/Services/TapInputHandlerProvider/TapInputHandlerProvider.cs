using Code.GamePlay;

namespace Code.Services.TapInputHandlerProvider
{
    public class TapInputHandlerProvider : ITapInputHandlerProvider
    {
        private TapInputHandler _tapInputHandler;

        public void SetInputHandler(TapInputHandler tapInputHandler) => 
            _tapInputHandler = tapInputHandler;

        public TapInputHandler GetTapInputHandler() =>
            _tapInputHandler;
    }
}