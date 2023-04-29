namespace Modules.Weapons.WeaponManager.Scripts
{
    public class WeaponStateEvent
    {
        public string WeaponName { get; set; }
        public int CurrentAmmo { get; set; }
        public int RemainingAmmo { get; set; }
        public float ReloadTime { get; set; }
        public float FireCooldown { get; set; }
        public WeaponStateEventType EventType{ get; set; }
    }

    public enum WeaponStateEventType
    {
        Status,
        ReloadStarted,
        ReloadFinished,
        FireWeapon,
        FireAlternate,
    }
}