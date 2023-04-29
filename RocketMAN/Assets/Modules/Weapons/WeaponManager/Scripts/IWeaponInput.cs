namespace Modules.Weapons.WeaponManager.Scripts
{
    public interface IWeaponInput
    {
        public delegate void SimpleWeaponEventTrigger();

        public event SimpleWeaponEventTrigger OnFireWeapon;
        public event SimpleWeaponEventTrigger OnReloadWeapon;

        public delegate void SwitchWeapon(string weapon);
        public event SwitchWeapon OnSwitchWeapon;

        public event SimpleWeaponEventTrigger OnAlternateFire;
    }
}