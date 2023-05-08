using UnityEngine;

namespace Modules.ScoreManager.Scripts
{
    public class AmmoUsage
    {
        [field: SerializeField]
        public string Map { get; set; }

        [field: SerializeField]
        public int Amount { get; set; }
    }
}