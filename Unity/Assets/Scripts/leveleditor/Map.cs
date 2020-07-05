using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Map
{
    public int SIZE = 50;
    public int money;
    public string filename;
    private Camera camera;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private GameObject map;

    private Vector3[] mapVertices;
    private Color[] localDifficulty;
    
    public float MinHeight { get; set; }
    public float MaxHeight { get; set; }
    public Map(Camera camera, GameObject map, int size, int money, string filename)
    {
        this.SIZE = size;
        this.money = money;
        this.filename = filename;
        this.map = map;
        this.meshCollider = map.GetComponent<MeshCollider>();
        this.meshFilter = map.GetComponent<MeshFilter>();
        this.camera = camera;

        MinHeight = -10;
        MaxHeight = 10;
        
        init();
    }

     public void UpdateMap(EditorMap.ToolsEnum currentTool, int radiusTool, int amplitude)
     {
         RaycastHit hit;
         if (!EventSystem.current.IsPointerOverGameObject())
         {
             if (meshCollider.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 1000))
             {
                 if (Input.GetKey(KeyCode.Mouse0))
                 {
                     switch (currentTool)
                     {
                         case EditorMap.ToolsEnum.MAP_BRUSH:
                             UpdateHeighmapRegion(hit, radiusTool, amplitude, 0);
                             break;
                         case EditorMap.ToolsEnum.DIFFICULTY_PAINTING:
                             UpdateLocalDifficulty(hit, radiusTool, amplitude, 0);
                             break;
                     }
                 }
                 else if (Input.GetKey(KeyCode.Mouse1))
                 {
                     switch (currentTool)
                     {
                         case EditorMap.ToolsEnum.MAP_BRUSH:
                             UpdateHeighmapRegion(hit, radiusTool, amplitude, 1);
                             break;
                         case EditorMap.ToolsEnum.DIFFICULTY_PAINTING:
                             UpdateLocalDifficulty(hit, radiusTool, amplitude, 1);
                             break;
                     }
                 }
             }
         }
    /*
         if (Input.GetKeyDown(KeyCode.L))
         {
             
         }*/
     }

     public void save()
     {
         Rule r = new Rule(mapVertices, localDifficulty, (byte)SIZE, money);
         Rule.saveLevel(filename, r);
     }

     [Obsolete]
     static public Map load(Camera camera, GameObject mapP, string filename)
     {
         Rule r = Rule.readLevel(filename);
         Map map = new Map(camera, mapP, r.size, r.maxBudget, filename);
         map.mapVertices = r.loadHeightmap(map.mapVertices);
         map.localDifficulty = r.loadDifficulty(map.localDifficulty);
         map.meshFilter.mesh.vertices = map.mapVertices;
         map.meshCollider.sharedMesh.vertices = map.mapVertices;
         map.meshFilter.mesh.colors = map.localDifficulty;

         return map;
     }

     public void UpdateHeighmapRegion(RaycastHit hit,int radiusTool, int amplitude, int mode)
     {
         for (int x = -radiusTool; x <= radiusTool; x++)
         {
             for (int z = -radiusTool; z <= radiusTool; z++)
             {
                 int globalX = (int)hit.point.x + x;
                 int globalZ = (int) hit.point.z + z;
                 float heightmapOffset = amplitude * (float)Math.Exp(-1f/radiusTool*(x * x + z * z));
                 if(0 <= globalX && globalX <= SIZE-1 && 0 <= globalZ && globalZ <= SIZE-1)
                     if(mode == 0 && MaxHeight > mapVertices[globalX * (SIZE) + globalZ ].y)
                        mapVertices[globalX * (SIZE) + globalZ ].y += heightmapOffset*Time.deltaTime;
                    else if(mode == 1 && MinHeight < mapVertices[globalX * (SIZE) + globalZ ].y)
                         mapVertices[globalX * (SIZE) + globalZ ].y -= heightmapOffset*Time.deltaTime;
             }
         }
         meshFilter.mesh.vertices = mapVertices;
         meshCollider.sharedMesh.vertices = mapVertices;
         meshFilter.mesh.RecalculateNormals();
     }

     public void UpdateLocalDifficulty(RaycastHit hit,int radiusTool, int amplitude, int mode)
     {
         for (int x = -radiusTool; x <= radiusTool; x++)
         {
             for (int z = -radiusTool; z <= radiusTool; z++)
             {
                 if (Math.Sqrt(x * x + z * z) <= radiusTool)
                 {
                     int globalX = (int) hit.point.x + x;
                     int globalZ = (int) hit.point.z + z;
                     float heightmapOffset =  amplitude * (float) Math.Exp(-1f / radiusTool * (x * x + z * z));
                     if (0 <= globalX && globalX <= SIZE -1 && 0 <= globalZ && globalZ <= SIZE -1)
                         if(mode==0)
                            localDifficulty[globalX * (SIZE) + globalZ].r += heightmapOffset*Time.deltaTime/255f;
                        else if(mode==1)
                             localDifficulty[globalX * (SIZE) + globalZ].r -= heightmapOffset*Time.deltaTime/255f;
                 }
             }
         }
         meshFilter.mesh.colors = localDifficulty;
     }

     public void updateMapProperties(float minHeight, float maxHeight)
     {
         MinHeight = minHeight;
         MaxHeight = maxHeight;
     }
     
     public void init()
    {
        var generalMesh = new Mesh();

        generalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mapVertices = getVertices(SIZE);
        generalMesh.vertices = mapVertices;
        generalMesh.triangles = getIndices(SIZE);
        generalMesh.uv = getUv(SIZE);
        
        localDifficulty = getDifficulty(SIZE);
        generalMesh.colors = localDifficulty;

        generalMesh.RecalculateNormals();
        
        meshFilter.mesh = generalMesh;
        meshCollider.sharedMesh = generalMesh;
    }

     private Vector2[] getUv(int size)
     {
         Vector2[] result = new Vector2[size*size];

         for(int i = 0;i < size; i++)
         {
             for(int j = 0;j < size; j++)
             {
                 result[i*size + j] = new Vector2(i/10f,  j/10f);
             }
         }

         return result;
     }

     public Vector3[] getVertices(int size)
    {
        Vector3[] result = new Vector3[size*size];

        for(int i = 0;i <size; i++)
        {
            for(int j = 0;j < size; j++)
            {
                result[i*size + j] = new Vector3(i, 0, j);
            }
        }

        return result;
    }
    public Color[] getDifficulty(int size)
    {
        Color[] result = new Color[size*size];

        for(int i = 0;i < size; i++)
        {
            for(int j = 0;j < size; j++)
            {
                result[i*size + j] = new Color(0,0,0);
            }
        }

        return result;
    }
    public int[] getIndices(int size)
    {
        int[] result = new int[6 * (size) * (size)];
        int c = 0;
        int vertexC = 0;
        for (int i = 0; i < size-1; i++)
        {
            for (int j = 0; j < size-1; j++)
            {

                result[c++] = vertexC +1;
                result[c++] = (size - 1 ) + vertexC + 1;
                result[c++] = vertexC;

                result[c++] = vertexC + 1;
                result[c++] = (size - 1) + vertexC + 2;
                result[c++] = (size - 1) + vertexC +1;

                vertexC++;
            }

            vertexC++;
        }

        return result;
    }
}
