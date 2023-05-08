using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.ScoreManager.Scripts
{
    [CreateAssetMenu(menuName = "Ammo Usages", fileName = "AmmoUsages")]
    public class AmmoUsages : ScriptableObject
    {
        public List<AmmoUsage> AllAmmoUsages = new();

        public void AddAmmoUsage(AmmoUsage ammoUsage)
        {
            AllAmmoUsages.Add(ammoUsage);
        }

        public void AddAmmoUsages(List<AmmoUsage> ammoUsages)
        {
            AllAmmoUsages.AddRange(ammoUsages);
        }

        public Dictionary<string, int> ToDictionary()
        {
            Dictionary<string, int> usageDict = new();
            AllAmmoUsages.ForEach(u => { usageDict.Add(u.Map, u.Amount); });
            return usageDict;
        }

        public bool UsageForMapAlreadyExists(string mapName)
        {
            return AllAmmoUsages.Any(u => u.Map == mapName);
        }

        public void UpdateUsageForMap(string mapName, int updatedAmount)
        {
            var existing = AllAmmoUsages.FirstOrDefault(u => u.Map == mapName);
            if (existing == null)
            {
                return;
            }
         
            existing.Amount = updatedAmount;
        }
    }
}