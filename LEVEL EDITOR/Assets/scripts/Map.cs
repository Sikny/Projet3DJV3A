using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Map
{
    public const int SIZE = 100;
    private Camera camera;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private GameObject map;

    private Vector3[] mapVertices;
    private Color[] localDifficulty;

    public Map(Camera camera, GameObject map)
    {

        this.map = map;
        this.meshCollider = map.GetComponent<MeshCollider>();
        this.meshFilter = map.GetComponent<MeshFilter>();
        this.camera = camera;
        
        init();
    }

     public void UpdateMap(EditorMap.ToolsEnum currentTool, int radiusTool, int amplitude)
     {
         RaycastHit hit;
         if(meshCollider.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit,1000)){
             if (Input.GetKey(KeyCode.Mouse0))
             {
                 switch (currentTool)
                 {
                     case EditorMap.ToolsEnum.MAP_BRUSH:
                         UpdateHeighmapRegion(hit, radiusTool, amplitude,0);
                         break;
                     case EditorMap.ToolsEnum.DIFFICULTY_PAINTING:
                         UpdateLocalDifficulty(hit, radiusTool, amplitude,0);
                         break;
                 }
             }else if (Input.GetKey(KeyCode.Mouse1))
             {
                 switch (currentTool)
                 {
                     case EditorMap.ToolsEnum.MAP_BRUSH:
                         UpdateHeighmapRegion(hit, radiusTool, amplitude,1);
                         break;
                     case EditorMap.ToolsEnum.DIFFICULTY_PAINTING:
                         UpdateLocalDifficulty(hit, radiusTool, amplitude,1);
                         break;
                 }
             }
         }
     }

     public void UpdateHeighmapRegion(RaycastHit hit,int radiusTool, int amplitude, int mode)
     {
         for (int x = -radiusTool; x <= radiusTool; x++)
         {
             for (int z = -radiusTool; z <= radiusTool; z++)
             {
                 int globalX = (int)hit.point.x + SIZE + x;
                 int globalZ = (int) hit.point.z + SIZE + z;
                 float heightmapOffset = amplitude * (float)Math.Exp(-1f/radiusTool*(x * x + z * z));
                 if(0 <= globalX && globalX <= SIZE*2 && 0 <= globalZ && globalZ <= SIZE*2)
                     if(mode == 0)
                        mapVertices[globalX * (SIZE*2+1) + globalZ ].y += heightmapOffset*Time.deltaTime;
                    else if(mode == 1)
                         mapVertices[globalX * (SIZE*2+1) + globalZ ].y -= heightmapOffset*Time.deltaTime;
             }
         }
         meshFilter.mesh.vertices = mapVertices;
         meshCollider.sharedMesh.vertices = mapVertices;
     }

     public void UpdateLocalDifficulty(RaycastHit hit,int radiusTool, int amplitude, int mode)
     {
         for (int x = -radiusTool; x <= radiusTool; x++)
         {
             for (int z = -radiusTool; z <= radiusTool; z++)
             {
                 if (Math.Sqrt(x * x + z * z) <= radiusTool)
                 {
                     int globalX = (int) hit.point.x + SIZE + x;
                     int globalZ = (int) hit.point.z + SIZE + z;
                     float heightmapOffset =  amplitude * (float) Math.Exp(-1f / radiusTool * (x * x + z * z));
                     if (0 <= globalX && globalX <= SIZE * 2 && 0 <= globalZ && globalZ <= SIZE * 2)
                         if(mode==0)
                            localDifficulty[globalX * (SIZE * 2 + 1) + globalZ].r += heightmapOffset*Time.deltaTime;
                        else if(mode==1)
                             localDifficulty[globalX * (SIZE * 2 + 1) + globalZ].r -= heightmapOffset*Time.deltaTime;
                 }
             }
         }
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
