using UnityEngine;

namespace Utility
{
    public static class RayCastUtil
    {
        public static Ray GetRayToCenterOfScreen(Camera mainCamera)
        {
            return mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        }
    }
}