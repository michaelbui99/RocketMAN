using System.Collections.Generic;
using UnityEngine;

namespace Modules.ScoreManager.Timer.Scripts
{
    [CreateAssetMenu(menuName = "Moments", fileName = "Moments")]
    public class Moments: ScriptableObject
    {
        public Dictionary<string, float> MomentsList;
    }
}