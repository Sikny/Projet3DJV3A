using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_worldgen : MonoBehaviour
{

    public int x=20, y=20;    
    
    void Start()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(i,0,j);
                MeshRenderer meshRenderer = cube.GetComponent<MeshRenderer>();
                meshRenderer.material.SetColor("_BaseColor", (i + j) % 2 == 0 ? Color.black : Color.white);
                cube.transform.SetParent(gameObject.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
