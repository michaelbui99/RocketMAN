using UnityEngine;

namespace Modules.Timer.Scripts
{
    [CreateAssetMenu(menuName = "Timer State", fileName = "TimeState")]
    public class TimerState : ScriptableObject
    {
        public float Time = 0f;
    }
}