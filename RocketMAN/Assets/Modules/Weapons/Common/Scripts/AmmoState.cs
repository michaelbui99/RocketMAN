using UnityEngine;

namespace Modules.Weapons.Common.Scripts
{
    [CreateAssetMenu(menuName = "Ammo State")]
    public class AmmoState: ScriptableObject
    {
        public int CurrentAmmoCount;
        public int RemainingAmmoCount;
    }
}