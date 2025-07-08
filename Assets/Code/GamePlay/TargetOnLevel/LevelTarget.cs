using UnityEngine;

namespace Code.GamePlay.TargetOnLevel
{
    public class LevelTarget : MonoBehaviour
    {
        public void Initialize(LevelTargetPositionContainer containerProvider)
        {
            transform.position = containerProvider.transform.position;
        }
    }
}