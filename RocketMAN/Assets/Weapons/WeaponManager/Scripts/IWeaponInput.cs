namespace Weapons.WeaponManager
{
    public interface IWeaponInput
    {
        public delegate void FireWeapon();

        public event FireWeapon OnFireWeapon;

        public delegate void SwitchWeapon(string weapon);
        public event SwitchWeapon OnSwitchWeapon;
    }
}