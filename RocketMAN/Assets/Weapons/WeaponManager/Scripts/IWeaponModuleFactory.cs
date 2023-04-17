using System;
using Weapons.RocketLauncher.Scripts;

namespace Weapons.WeaponManager.Scripts
{
    public interface IWeaponModuleFactory
    {
        public IWeaponModule Create(string weapon);
        public IWeaponModule GetDefault();
    }

    public class WeaponModuleFactory : IWeaponModuleFactory
    {
        public IWeaponModule Create(string weapon)
        {
            return weapon switch
            {
                SupportedWeaponModules.RocketLauncher => new RocketLauncherModule(),
                _ => throw new ArgumentException("Not supported")
            };
        }

        public IWeaponModule GetDefault()
        {
            return new RocketLauncherModule();
        }
    }
}