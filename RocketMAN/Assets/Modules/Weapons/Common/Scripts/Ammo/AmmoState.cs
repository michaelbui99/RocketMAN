using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Ammo
{
    [CreateAssetMenu(menuName = "Ammo State")]
    public class AmmoState: ScriptableObject
    {
        public int CurrentAmmoCount;
        public int RemainingAmmoCount;
    }
}