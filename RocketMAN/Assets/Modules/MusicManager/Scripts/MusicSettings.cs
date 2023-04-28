using UnityEngine;

namespace Modules.MusicManager.Scripts
{
    [CreateAssetMenu(menuName = "Music Settings", fileName = "MusicSettings")]
    public class MusicSettings: ScriptableObject
    {
        public float AudioLevel = 100f;
        public bool DisableMusic = false;
    }
}