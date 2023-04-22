using System;
using Modules.Events;
using UnityEngine;

namespace Modules.AmmoCrate.Scripts
{
    public class AmmoCrate : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameEvent ammoPickupEvent;
        
        [Header("Settings")]
        [SerializeField]
        private bool persistant;
        [SerializeField]
        private int reloadUnitsToRestore;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
            
            ammoPickupEvent.Raise(reloadUnitsToRestore);
            
            if (!persistant)
            {
                Destroy(gameObject);
            }
        }
    }
}