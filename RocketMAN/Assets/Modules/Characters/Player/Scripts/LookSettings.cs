using UnityEngine;

namespace Modules.Characters.Player.Scripts
{
    [CreateAssetMenu(menuName = "Player Look Settings")]
    public class LookSettings: ScriptableObject
    {
        public float VerticalMouseSensitivity = 0.8f;
        public float HorizontalMouseSensitivity = 0.8f;
    }
}