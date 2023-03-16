using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject player;

    [Header("Position Settings")]
    [SerializeField]
    private AttachSide side;

    private const float HorizontalOffset = 0.10f;
    private const float VerticalOffset = 0.5f;

    private void LateUpdate()
    {
        transform.position = GetTargetPosition(side);
    }

    private Vector3 GetTargetPosition(AttachSide attachSide)
    {
        return attachSide switch
        {
            AttachSide.Right => GetRightAttachPosition(),
            AttachSide.Left => throw new NotImplementedException(),
            _ => throw new ArgumentException("Unsupported side")
        };
    }

    private Vector3 GetRightAttachPosition()
    {
        var currentPlayerPosition = player.transform.position;
        var playerWidth = player.transform.localScale.x;
        return new Vector3(currentPlayerPosition.x + playerWidth/2  + HorizontalOffset, currentPlayerPosition.y - VerticalOffset, currentPlayerPosition.z);
    }

    public enum AttachSide
    {
        Left,
        Right
    }
}