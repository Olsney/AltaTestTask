using UnityEngine;

namespace Code.Infrastructure
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        private void Start() => 
            DontDestroyOnLoad(this);
    }
}