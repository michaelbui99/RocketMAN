using UnityEngine;

namespace Weapons.WeaponManager.Scripts
{
    public class CurrentWeapon
    {
        public GameObject instance{ get; set; }
        public AudioSource fireWeaponAudio{ get; set; }
    }
}