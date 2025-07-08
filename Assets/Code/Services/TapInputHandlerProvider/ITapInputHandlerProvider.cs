using Code.GamePlay;
using Code.GamePlay.InputHandler;

namespace Code.Services.TapInputHandlerProvider
{
    public interface ITapInputHandlerProvider
    {
        void SetInputHandler(TapInputHandler tapInputHandler);
        TapInputHandler GetTapInputHandler();
    }
}