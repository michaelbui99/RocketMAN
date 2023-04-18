using UnityEngine;
using Weapons.Common;

namespace Weapons.WeaponManager.Scripts
{
    public class CurrentWeapon
    {
        public GameObject instance{ get; set; }
        public IWeapon WeaponComponent{ get; set; }
    }
}