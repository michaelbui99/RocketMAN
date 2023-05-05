using UnityEngine;

namespace Modules.Characters.Player.Scripts
{
    [CreateAssetMenu(menuName = "Player Sensitivity Settings")]
    public class SensitivitySettings: ScriptableObject
    {
        public float VerticalMouseSensitivity = 0.8f;
        public float HorizontalMouseSensitivity = 0.8f;
    }
}