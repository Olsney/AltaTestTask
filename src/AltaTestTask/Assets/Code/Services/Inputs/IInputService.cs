namespace Code.Services.Inputs
{
    public interface IInputService
    {
        bool IsTapStarted();
        bool IsTapHeld();
        bool IsTapEnded();
    }
}