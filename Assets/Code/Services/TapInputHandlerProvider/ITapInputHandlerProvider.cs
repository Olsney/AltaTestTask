using Code.GamePlay;

namespace Code.Services.TapInputHandlerProvider
{
    public interface ITapInputHandlerProvider
    {
        void SetInputHandler(TapInputHandler tapInputHandler);
        TapInputHandler GetTapInputHandler();
    }
}