using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Animations;

public class UnitController : MonoBehaviour
{
    public Camera cam;
   
    public LayerMask groundMask;
   
   
    public float rotationSpeed = 5f;
    public float speed = 5f;
    
    private float rotationAngle;
    
    private Vector3 lookAtTarget;
    private Vector3 targetPosition;
    private Vector3 offsetPosition;

    private int rotationTolerance = 1;
    
    private Quaternion unitRotation;

    private bool isMoving = false;
    private bool isTurning = false;
    private bool isRight = false;

    private void Start()
    {
        offsetPosition = new Vector3(0f, this.gameObject.transform.localScale.y + 0.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
        }

        if (isTurning)
            Turn();
        if(isMoving)
            Move();  
    }

    private void SetTargetPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("hit : " + hit.transform.gameObject.layer);
            if (hit.transform.gameObject.layer == 9)
            {
                float xHit = Mathf.Floor(hit.transform.position.x);
                float zHit = Mathf.Floor(hit.transform.position.z);
                Debug.Log("xhit: " + xHit);
                Debug.Log("zhit:" + zHit);
            }

            //targetPosition = hit.point + offsetPosition;
            targetPosition = new Vector3(Mathf.Floor(hit.transform.position.x) + 0.5f, 0, Mathf.Floor(hit.transform.position.z)+0.5f) + offsetPosition; 


            lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y,
                targetPosition.z - transform.position.z);
            
            //Vector of unit to point 
            Vector3 unitToTarget = (targetPosition - transform.position);
            unitToTarget.Normalize();
            //Dot product of the two vectors and Acos 
            rotationAngle = (Mathf.Acos(Vector3.Dot(unitToTarget, gameObject.transform.forward)) * (180/Mathf.PI));
            isRight = Vector3.Dot(unitToTarget, gameObject.transform.right) > 0;

            Debug.Log("rotation angle:" + rotationAngle);
            Debug.Log("unitToTarget" + unitToTarget);
            Debug.Log("transform.front" + gameObject.transform.forward);
            isMoving = true;
            isTurning = true;
        }
    }

    private void Move()
    {

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
    }

    private void Turn()
    {
        int factor = isRight ? 1 : -1;
        rotationAngle -= rotationSpeed * Time.deltaTime;
        
        gameObject.transform.Rotate(gameObject.transform.up, rotationSpeed * Time.deltaTime * factor);
        
        if (rotationAngle <= 0)
        {
            isTurning = false;
        }
    }
    
}
