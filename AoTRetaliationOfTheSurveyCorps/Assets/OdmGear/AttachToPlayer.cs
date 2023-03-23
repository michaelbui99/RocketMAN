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
    
    [SerializeField]
    private float horizontalOffset = 0.10f;
    
    [SerializeField]
    private float verticalOffset = 0.5f;

    private void LateUpdate()
    {
        transform.position = GetAttachPosition(side);
    }

    private Vector3 GetAttachPosition(AttachSide attachSide)
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
        return new Vector3(currentPlayerPosition.x + playerWidth/2  + horizontalOffset, currentPlayerPosition.y - verticalOffset, currentPlayerPosition.z);
    }
    private Vector3 GetLeftAttachPosition()
    {
        var currentPlayerPosition = player.transform.position;
        var playerWidth = player.transform.localScale.x;
        return new Vector3(currentPlayerPosition.x - playerWidth/2  - horizontalOffset, currentPlayerPosition.y - verticalOffset, currentPlayerPosition.z);
    }

    public enum AttachSide
    {
        Left,
        Right
    }
}