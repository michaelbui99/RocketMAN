using System;
using System.Collections.Generic;
using Modules.Characters.AmmoBot.Scripts.States;
using Modules.Events;
using Modules.Weapons.WeaponManager.Scripts;
using UnityEngine;

namespace Modules.Characters.AmmoBot.Scripts
{
    public class AmmoBotAI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject ammoCratePrefab;

        [field: SerializeField]
        public GameObject Player { get; set; }

        [field: SerializeField]
        public Transform ReturnToBoundsPoint { get; set; }

        [field: Header("Settings")]
        [field: SerializeField]
        public float MovementSpeed { get; set; }

        [field: SerializeField]
        public float MinDistanceToPlayer { get; set; }

        public CharacterController CharacterController { get; set; }
        public IAmmoBotState State { get; private set; }
        public List<GameObject> SpawnedAmmoCrates { get; private set; } = new();

        public WeaponStateEvent CurrentKnownWeaponState { get; set; } =
            new() {CurrentAmmo = 1, RemainingAmmo = 1};

        public DialogController DialogController { get; private set; }

        private GameEventObserver _weaponStateObserver;

        private bool _grounded = false;

        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();
            DialogController = GetComponent<DialogController>();
            _weaponStateObserver = GetComponent<GameEventObserver>();

            _weaponStateObserver.RegisterCallback(UpdateKnownWeaponState);
        }

        private void Start()
        {
            SwitchTo(new IdleState());
        }

        private void Update()
        {
        }

        private void OnDestroy()
        {
            _weaponStateObserver.UnregisterCallback(UpdateKnownWeaponState);
        }

        public void SpawnAmmoCrate(Vector3 position)
        {
            GameObject ammoCrate = Instantiate(ammoCratePrefab);
            ammoCrate.transform.position = position;
            SpawnedAmmoCrates.Add(ammoCrate);
        }

        public void SwitchTo(IAmmoBotState state)
        {
            var stateName = state.GetType().Name;
            Debug.Log($"Current State: {stateName}");
            State = state;
            StartCoroutine(State.Act(this));
        }


        private void UpdateKnownWeaponState(object weaponState)
        {
            CurrentKnownWeaponState = (WeaponStateEvent) weaponState;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _grounded = collision.gameObject.CompareTag("Ground");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AmmoBotOOB"))
            {
                SwitchTo(new PreventOutOfBoundsState());
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _grounded = false;
            }
        }
    }
}