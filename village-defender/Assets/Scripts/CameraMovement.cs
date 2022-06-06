using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(-5, 5, -5);

    public float smoothSpeed = 0.125f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = target.position + offset; 
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);

        transform.position = smoothPos;
    }
}
