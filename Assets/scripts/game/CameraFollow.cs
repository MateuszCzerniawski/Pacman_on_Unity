using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] public Vector3 offset=new Vector3(0,0,-1);
    [SerializeField] public float smooth=0.125f;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position,target.transform.position+offset,smooth);
    }
}
