using OdmGear.GrappleHooks.Scripts;
using UnityEngine;
using Utility;

namespace OdmGear.SmartAim.Scripts
{
    public class SmartAim : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject indicatorPrefab;

        [SerializeField]
        private GrappleHookSettings globalHookSettings;

        private GameObject _indicatorInstance;
        private Vector3 _indicatorPosition;
        private Camera _mainCamera;
        private bool _isLookingDirectlyAtHookable = false;


        private void Start()
        {
            _mainCamera = Camera.main;
            _indicatorInstance = Instantiate(indicatorPrefab);
            _indicatorInstance.SetActive(false);
        }

        private void Update()
        {
            var drawPosition = TryFindHookAnchorPoint();

            if (drawPosition.HasValue)
            {
                _indicatorPosition = drawPosition.Value;
            }

            var shouldDrawIndicator = drawPosition.HasValue && !_isLookingDirectlyAtHookable;
            if (!shouldDrawIndicator)
            {
                _indicatorInstance.SetActive(false);
                return;
            }

            DrawIndicator();
        }

        private Vector3? TryFindHookAnchorPoint()
        {
            Ray ray = RayCastUtil.GetRayToCenterOfScreen(_mainCamera);
            if (!Physics.Raycast(ray, out RaycastHit hit, globalHookSettings.MaxDistanceInUnits,
                    globalHookSettings.HookableLayers))
            {
                _isLookingDirectlyAtHookable = false;
                return !SmartAimHit(ray, out hit, globalHookSettings)
                    ? null
                    : hit.point;
            }

            _isLookingDirectlyAtHookable = true;
            return hit.point;
        }

        private void DrawIndicator()
        {
            _indicatorInstance.SetActive(true);
            _indicatorInstance.transform.position = _indicatorPosition;
        }

        public static bool SmartAimHit(Ray ray, out RaycastHit hitInfo, GrappleHookSettings settings)
        {
            return Physics.SphereCast(ray, settings.SmartAimRadius, out hitInfo,
                settings.MaxDistanceInUnits,
                settings.HookableLayers);
        }
    }
}