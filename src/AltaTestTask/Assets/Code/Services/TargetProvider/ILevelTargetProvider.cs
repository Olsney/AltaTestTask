using Code.GamePlay.TargetOnLevel;

namespace Code.Services.TargetProvider
{
    public interface ILevelTargetProvider
    {
        LevelTarget Instance { get; set; }
    }
}