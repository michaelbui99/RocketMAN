using System.Collections.Generic;

namespace Modules.Weapons.Common.Scripts.Launchers
{
    public interface IProjectileLauncher
    {
        public void Launch();
        public List<IProjectile> GetActiveProjectiles();
    }
}