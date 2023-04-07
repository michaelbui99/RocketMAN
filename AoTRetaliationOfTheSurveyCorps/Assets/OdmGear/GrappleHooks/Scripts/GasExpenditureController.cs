using System;
using OdmGear.Gas;
using UnityEngine;

namespace OdmGear.GrappleHooks.Scripts
{
    public class GasExpenditureController : MonoBehaviour, IGasExpenditureController
    {
        [Header("References")]
        [SerializeField]
        private GasController gasController;

        [SerializeField]
        private GrappleHookSettings globalHookSettings;

        private IHitDetection _hitDetection;

        private void Awake()
        {
            _hitDetection = gameObject.GetComponent<IHitDetection>();
        }

        private void Start()
        {
            _hitDetection.OnHookLaunchedEvent += SpendHookLaunchCost;
            _hitDetection.OnHitEvent += SpendOnHookPropulsionCost;
        }

        private void OnDestroy()
        {
            _hitDetection.OnHookLaunchedEvent -= SpendHookLaunchCost;
            _hitDetection.OnHitEvent -= SpendOnHookPropulsionCost;
        }

        public void SpendHookLaunchCost(Vector3 direction)
        {
            if (HasGasForHookLaunch())
            {
                gasController.SpendGas(globalHookSettings.LaunchHookGasCost);
            }
        }

        public void SpendOnHookPropulsionCost(RaycastHit hit)
        {
            if (HasGasForHookLaunch())
            {
                gasController.SpendGas(globalHookSettings.OnHookPropulsionCost);
            }
        }

        public bool HasGasForHookLaunch()
        {
            return !gasController.GasTankEmpty();
        }
    }
}