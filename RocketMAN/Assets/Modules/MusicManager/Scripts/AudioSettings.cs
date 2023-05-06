using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.MusicManager.Scripts
{
    [CreateAssetMenu(menuName = "Audio Settings", fileName = "AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        public float MusicVolume = 100f;
        public bool DisableMusic = false;

        public float GameVolume = 100f;
        public bool DisableGameSound = false;
    }
}