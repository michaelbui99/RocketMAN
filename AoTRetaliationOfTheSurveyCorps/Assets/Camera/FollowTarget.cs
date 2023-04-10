using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget: MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject followTarget;

    private void LateUpdate()
    {
        var cameraTransform = transform;
        cameraTransform.position = followTarget.transform.position;
        cameraTransform.forward = followTarget.transform.forward;
    }
}