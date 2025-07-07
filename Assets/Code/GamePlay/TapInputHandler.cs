using Code.Services.Inputs;
using UnityEngine;
using Zenject;

namespace Code.GamePlay
{
    public class TapInputHandler : MonoBehaviour
    {
        [Inject]
        public void Construct(IInputService inputService)
        {
            
        }
    }
}