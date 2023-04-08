using System;
using OdmGear.Gas;
using UnityEngine;

namespace OdmGear.GrappleHooks.Scripts
{
    public class GasExpenditureController : MonoBehaviour
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

        public void SpendHookLaunchCost(Vector3 direction, float distance)
        {
            if (!gasController.HasEnoughGas(globalHookSettings.LaunchHookGasCost))
            {
                return;
            }

            gasController.SpendGas(globalHookSettings.LaunchHookGasCost);
        }

        public void SpendOnHookPropulsionCost(RaycastHit hit)
        {
            if (!gasController.HasEnoughGas(globalHookSettings.OnHookPropulsionCost))
            {
                return;
            }

            gasController.SpendGas(globalHookSettings.OnHookPropulsionCost);
        }
    }
}