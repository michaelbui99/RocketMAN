using System.Collections.Generic;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts
{
    public interface IProjectileLauncher
    {
        public void Launch();
        public List<IProjectile> GetActiveProjectiles();
    }
}