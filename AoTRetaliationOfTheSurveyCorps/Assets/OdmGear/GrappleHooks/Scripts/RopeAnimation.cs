using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OdmGear.GrappleHooks.Scripts
{
    // NOTE: (mibui 2023-04-07) This class is based on the tutorial "Animate LineRenderer in Unity"
    public class RopeAnimation
    {
        public IEnumerator AnimateRope(LineRenderer lineRenderer, float animationDuration)
        {
            float startTime = Time.time;

            Vector3 startPosition = lineRenderer.GetPosition(0);
            Vector3 endPosition = lineRenderer.GetPosition(1);

            Vector3 position = startPosition;

            while (position != endPosition)
            {
                float time = (Time.time - startTime) / animationDuration;
                position = Vector3.Lerp(startPosition, endPosition, time);
                lineRenderer.SetPosition(1, position);
                yield return null;
            }
        }
    }
}