using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Characters.AmmoBot.Scripts.States;
using Modules.Events;
using Modules.Odm.Scripts;
using Modules.Weapons.WeaponManager.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Utility;

namespace Modules.Characters.AmmoBot.Scripts
{
    public class AmmoBotAI : MonoBehaviour
    {
        [FormerlySerializedAs("ammoCratePrefab")]
        [Header("References")]
        [SerializeField]
        private GameObject supplyCratePrefab;

        [field: SerializeField]
        public GameObject Player { get; set; }

        [field: Header("Movement Settings")]
        [field: SerializeField]
        public float MovementSpeed { get; set; }

        [field: SerializeField]
        public float RotationSpeed { get; set; }

        [field: SerializeField]
        public float MinDistanceToPlayer { get; set; }

        [field: Header("Resupply Settings")]
        [field: SerializeField]
        public float GasPerSupplyCrate { get; set; }

        [field: SerializeField]
        public int ReloadUnitsPerSupplyCrate { get; set; }

        [field: SerializeField]
        public int CratesToDropOnResupply { get; set; }

        [field: SerializeField]
        public int ClearAllCratesThreshold { get; set; }

        [field: SerializeField]
        public bool DebugOn { get; set; } = false;


        public NavMeshAgent Agent { get; private set; }
        public List<GameObject> SpawnedSupplyCrates { get; private set; } = new();

        public WeaponStateEvent CurrentKnownWeaponState { get; private set; } =
            new() {CurrentAmmo = 1, RemainingAmmo = 1};

        public GasStateEvent CurrentKnownGasState { get; private set; } = new()
        {
            CurrentLevel = 420,
            MaxCapacity = 420
        };

        public DialogController DialogController { get; private set; }

        private IAmmoBotState _state;
        private GameEventObserver _weaponStateObserver;
        private GameEventObserver _gasStateObserver;
        private bool _grounded = false;

        private void Awake()
        {
            DialogController = GetComponent<DialogController>();
            Agent = GetComponent<NavMeshAgent>();
            _weaponStateObserver = GameObject.Find("WeaponStateObserver").GetComponent<GameEventObserver>();
            _gasStateObserver = GameObject.Find("GasStateObserver").GetComponent<GameEventObserver>();

            _weaponStateObserver.RegisterCallback(UpdateKnownWeaponState);
            _gasStateObserver.RegisterCallback(UpdateKnownGasState);
        }

        private void Start()
        {
            Agent.speed = MovementSpeed;
            Agent.angularSpeed = RotationSpeed;
            SwitchTo(new IdleState());
        }

        private void OnDestroy()
        {
            _weaponStateObserver.UnregisterCallback(UpdateKnownWeaponState);
            _gasStateObserver.UnregisterCallback(UpdateKnownGasState);
        }

        public void SpawnSupplyCrate(Vector3 position)
        {
            CleanSupplyCratesList();
            if (SpawnedSupplyCrates.Count >= CratesToDropOnResupply)
            {
                return;
            }

            var supplyCrate = Instantiate(supplyCratePrefab);
            var component = supplyCrate.GetComponentInChildren<SupplyCrate.Scripts.SupplyCrate>() switch
            {
                null => supplyCrate.AddComponent<SupplyCrate.Scripts.SupplyCrate>(),
                SupplyCrate.Scripts.SupplyCrate c => c,
            };

            component
                .SetGasToRestore(GasPerSupplyCrate)
                .SetReloadUnitsToRestore(ReloadUnitsPerSupplyCrate)
                .SetPersistent(false);

            supplyCrate.transform.position = position + Vector3.up * 1f;
            SpawnedSupplyCrates.Add(supplyCrate);
            if (DebugOn)
            {
                Debug.Log($"Spawning Crate at: {JsonUtility.ToJson(position)}");
            }
        }

        public void ClearAllSupplyCrates()
        {
            SpawnedSupplyCrates.ForEach(Destroy);
            SpawnedSupplyCrates.Clear();
        }

        public void SwitchTo(IAmmoBotState state)
        {
            if (DebugOn)
            {
                var stateName = state.GetType().Name;
                Debug.Log($"Current State: {stateName}");
            }

            _state = state;
            StartCoroutine(_state.Act(this));
        }

        public void LookAtPlayer()
        {
            var playerPositionToLook = Player.transform.position - transform.position;
            playerPositionToLook.y = 0;
            var rotation = Quaternion.LookRotation(playerPositionToLook);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
        }

        public bool PlayerNeedsResupply()
        {
            return (CurrentKnownWeaponState.CurrentAmmo == 0 && CurrentKnownWeaponState.RemainingAmmo == 0) ||
                   CurrentKnownGasState.CurrentLevel == 0;
        }


        private void UpdateKnownWeaponState(object weaponState)
        {
            CurrentKnownWeaponState = weaponState.Cast<WeaponStateEvent>();
        }

        private void UpdateKnownGasState(object gasState)
        {
            CurrentKnownGasState = gasState.Cast<GasStateEvent>();
        }


        private void OnCollisionEnter(Collision collision)
        {
            _grounded = collision.gameObject.CompareTag("Ground");
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _grounded = false;
            }
        }

        private void CleanSupplyCratesList()
        {
            SpawnedSupplyCrates
                .Where(c => c.gameObject == null)
                .ToList()
                .ForEach(g => SpawnedSupplyCrates.Remove(g));
        }
    }
}