using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public float smoothSpeed = 0.1F;

    private void Start()
    {
        // You can also specify your own offset from inspector as it is public variable
        // offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
    	smoothSpeed = Mathf.Lerp(smoothSpeed, 100F, 2F * Time.deltaTime);
    
        SmoothFollow();   
    }

    public void SmoothFollow()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 smoothFollow = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        transform.position = smoothFollow;
        //transform.LookAt(target);
    }
}
