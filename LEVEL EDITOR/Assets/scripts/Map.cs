using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    public const int SIZE = 100;
    public const int RADIUSCURVE = 20;
    public const int AMPLITUDE = 10;
    public Camera camera;
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    private GameObject map;

    private Vector3[] mapVertices;
    private Color[] localDifficulty;
    
     void Start()
    {
        init();
    }

     void Update()
     {
         RaycastHit hit;
         if (Input.GetKey(KeyCode.Mouse0) && meshCollider.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit,1000))
         {
             for (int x = -RADIUSCURVE; x <= RADIUSCURVE; x++)
             {
                 for (int z = -RADIUSCURVE; z <= RADIUSCURVE; z++)
                 {
                     int globalX = (int)hit.point.x + SIZE + x;
                     int globalZ = (int) hit.point.z + SIZE + z;
                     float heightmapOffset = AMPLITUDE * (float)Math.Exp(-1f/RADIUSCURVE*(x * x + z * z));
                     if(0 <= globalX && globalX <= SIZE*2 && 0 <= globalZ && globalZ <= SIZE*2)
                        mapVertices[globalX * (SIZE*2+1) + globalZ ].y += heightmapOffset*Time.deltaTime;
                 }
             }
             UpdateMesh();
         }

         if (Input.GetKey(KeyCode.Mouse1) && meshCollider.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit,1000))
         {
             for (int x = -RADIUSCURVE; x <= RADIUSCURVE; x++)
              {
                  for (int z = -RADIUSCURVE; z <= RADIUSCURVE; z++)
                  {
                      if (Math.Sqrt(x * x + z * z) <= RADIUSCURVE)
                      {
                          int globalX = (int) hit.point.x + SIZE + x;
                          int globalZ = (int) hit.point.z + SIZE + z;
                          float heightmapOffset =  (float) Math.Exp(-1f / RADIUSCURVE * (x * x + z * z));
                          if (0 <= globalX && globalX <= SIZE * 2 && 0 <= globalZ && globalZ <= SIZE * 2)
                              localDifficulty[globalX * (SIZE * 2 + 1) + globalZ].r += heightmapOffset*Time.deltaTime;
                      }
                  }
              }
              UpdateColor();
         }
     }

     public void UpdateMesh()
     {
         meshFilter.mesh.vertices = mapVertices;
         meshCollider.sharedMesh.vertices = mapVertices;
     }

     public void UpdateColor()
     {
         meshFilter.mesh.colors = localDifficulty;
     }

     public void init()
    {
        var generalMesh = new Mesh();

        generalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mapVertices = getVertices(SIZE);
        generalMesh.vertices = mapVertices;
        generalMesh.triangles = getIndices(SIZE);
        
        localDifficulty = getDifficulty(SIZE);
        generalMesh.colors = localDifficulty;

        generalMesh.RecalculateNormals();
        
        meshFilter.mesh = generalMesh;
        meshCollider.sharedMesh = generalMesh;
    }

    public Vector3[] getVertices(int size)
    {
        Vector3[] result = new Vector3[(size*2+1)*(size*2+1)];

        for(int i = 0;i <= size*2; i++)
        {
            for(int j = 0;j <= size*2; j++)
            {
                result[i*(size*2+1) + j] = new Vector3(i-size, 0, j-size);
            }
        }

        return result;
    }
    public Color[] getDifficulty(int size)
    {
        Color[] result = new Color[(size*2+1)*(size*2+1)];

        for(int i = 0;i <= size*2; i++)
        {
            for(int j = 0;j <= size*2; j++)
            {
                result[i*(size*2+1) + j] = new Color(0,0,0);
            }
        }

        return result;
    }
    public int[] getIndices(int size)
    {
        int[] result = new int[6 * (size*2) * (size*2)];
        int c = 0;
        int vertexC = 0;
        for (int i = 0; i < size*2; i++)
        {
            for (int j = 0; j < size*2; j++)
            {

                result[c++] = vertexC +1;
                result[c++] = (size * 2 ) + vertexC + 1;
                result[c++] = vertexC;

                result[c++] = vertexC + 1;
                result[c++] = (size * 2) + vertexC + 2;
                result[c++] = (size*2) + vertexC +1;

                vertexC++;
            }

            vertexC++;
        }

        return result;
    }
}
