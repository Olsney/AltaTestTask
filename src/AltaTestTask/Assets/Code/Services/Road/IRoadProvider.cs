using UnityEngine;

namespace Code.Services.Road
{
    public interface IRoadProvider
    {
        GameObject Instance { get; set; }
    }
}