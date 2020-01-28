using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class UnitMovement : MonoBehaviour
{
    NavMeshAgent agent; 
    // Start is called before the first frame update
    void Start()
    {
     
    }

    public void MoveToPoint(Vector3 point)
    {
        Debug.Log("Move to point");
        agent.SetDestination(point);
    }
}
