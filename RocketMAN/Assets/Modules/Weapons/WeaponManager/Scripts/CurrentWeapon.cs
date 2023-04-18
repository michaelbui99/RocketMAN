using Modules.Weapons.Common.Scripts;
using UnityEngine;

namespace Modules.Weapons.WeaponManager.Scripts
{
    public class CurrentWeapon
    {
        public GameObject instance{ get; set; }
        public IWeapon WeaponComponent{ get; set; }
    }
}