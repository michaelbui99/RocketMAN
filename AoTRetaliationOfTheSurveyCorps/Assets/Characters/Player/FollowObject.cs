using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    
    // Current offset for player is Position(0, -1.5, 3). Rotation on camera is (-1.5, 0, 0)
    private Vector3 _offset;
    
    // Start is called before the first frame update
    void Start()
    {
        _offset = target.transform.position - transform.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = target.transform.position - _offset;
    }
}
