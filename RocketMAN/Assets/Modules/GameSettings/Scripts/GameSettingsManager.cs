using Modules.Characters.Player.Scripts;
using Modules.MusicManager.Scripts;
using UnityEngine;
using UnityEngine.Audio;

namespace Modules.GameSettings.Scripts
{
    public class GameSettingsManager: MonoBehaviour
    {
        [Header("Player Settings Refs")]
        [SerializeField]
        private LookSettings playerLookSettings;
        [SerializeField]
        private MusicSettings musicSettings;

        [Header("Controls refs")]
        [SerializeField]
        private AudioMixer audioMixer;
    }
}