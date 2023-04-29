namespace Modules.Weapons.Common.Scripts.Ammo
{
    public class AmmoSettings
    {
        public ReloadBehaviour ReloadBehaviour{ get; set; }
        public int TotalReloadUnits { get; set; }
        public int AmmoPerReloadUnit { get; set; }
        public int ClipSize{ get; set; }
        public float ReloadTime { get; set; }
    }
}