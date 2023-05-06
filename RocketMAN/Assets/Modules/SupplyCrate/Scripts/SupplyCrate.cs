using System;
using Modules.Events;
using UnityEngine;

namespace Modules.AmmoCrate.Scripts
{
    public class SupplyCrate : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameEvent ammoPickupEvent;

        [SerializeField]
        private GameEvent restoreGasEvent;

        [Header("Settings")]
        [SerializeField]
        private bool persistant;

        [SerializeField]
        private int reloadUnitsToRestore;

        [SerializeField]
        private float gasToRestore;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            ammoPickupEvent.Raise(reloadUnitsToRestore);
            restoreGasEvent.Raise(gasToRestore);

            if (!persistant)
            {
                Destroy(gameObject);
            }
        }

        public SupplyCrate SetReloadUnitsToRestore(int amount)
        {
            reloadUnitsToRestore = amount;
            return this;
        }

        public SupplyCrate SetGasToRestore(float amount)
        {
            gasToRestore = amount;
            return this;
        }

        public SupplyCrate SetPersistent(bool isPersistent)
        {
            persistant = isPersistent;
            return this;
        }
    }
}