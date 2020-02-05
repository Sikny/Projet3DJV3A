using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public Transform centerPoint;
    public Vector3 offset; 


    public float zoomSpeed = 4f;
    public float minZoom = 4f;
    public float maxZoom = 50f;
    public float speed = 100f;
    public float pitch = 2f;

    private float currentZoom = 10f;
    private float currentYaw = 0f; 

    // Update is called once per frame
    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        if (Input.GetMouseButtonDown(1)) //right click
        {
            Debug.Log("Button is down");
            currentYaw -= Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Button released");
            currentYaw = 0;
        }
    }

    private void LateUpdate()
    {
        //transform.position = transform.position - offset * currentZoom;
        transform.LookAt(centerPoint.position + Vector3.up * pitch);
        transform.RotateAround(centerPoint.position,Vector3.up, currentYaw);   
    }
}
