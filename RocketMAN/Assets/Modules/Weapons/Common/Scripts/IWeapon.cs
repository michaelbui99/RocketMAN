namespace Modules.Weapons.Common.Scripts
{
    public interface IWeapon
    {
        public delegate void WeaponEventTrigger();

        public event WeaponEventTrigger ReloadStartedEvent;
        public event WeaponEventTrigger ReloadFinishedEvent;
        
        public void FireWeapon();
        public void AlternateFire();
        public void ReloadWeapon();

        public float GetFireCooldown();

        /// <summary>
        /// How much ammo can be expended before reloading
        /// </summary>
        public int GetCurrentAmmoCount();

        /// <summary>
        /// How much remaining ammo can can be used before not being able to fire the weapon
        /// </summary>
        public int GetRemainingAmmoCount();

        public void SetAmmoState(AmmoState ammoState);
        public void RestoreAmmo(int reloadUnits);
    }
}