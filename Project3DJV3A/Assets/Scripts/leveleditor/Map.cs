using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LevelEditor {
    public class Map
    {
        private readonly int _size;
        private readonly int _money;
        private readonly string _filename;
        private readonly Camera _camera;
        private readonly MeshFilter _meshFilter;
        private readonly MeshCollider _meshCollider;

        private Vector3[] _mapVertices;
        private Color[] _localDifficulty;

        private float MinHeight { get; set; }
        private float MaxHeight { get; set; }
        public Map(Camera camera, GameObject map, int size, int money, string filename)
        {
            _size = size;
            _money = money;
            _filename = filename;
            _meshCollider = map.GetComponent<MeshCollider>();
            _meshFilter = map.GetComponent<MeshFilter>();
            _camera = camera;

            MinHeight = -10;
            MaxHeight = 10;
        
            Init();
        }

        public void UpdateMap(EditorMap.ToolsEnum currentTool, int radiusTool, int amplitude)
        {
            RaycastHit hit;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (_meshCollider.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 1000))
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

        public void Save()
        {
            Rule.Rule r = new Rule.Rule(_mapVertices, _localDifficulty, (byte)_size, _money);
            Rule.Rule.SaveLevel(_filename, r);
        }

        [Obsolete]
        static public Map Load(Camera camera, GameObject mapP, string filename)
        {
            Rule.Rule r = Rule.Rule.ReadLevel(filename);
            Map map = new Map(camera, mapP, r.size, r.maxBudget, filename);
            map._mapVertices = r.LoadHeightmap(map._mapVertices);
            map._localDifficulty = r.LoadDifficulty(map._localDifficulty);
            map._meshFilter.mesh.vertices = map._mapVertices;
            map._meshCollider.sharedMesh.vertices = map._mapVertices;
            map._meshFilter.mesh.colors = map._localDifficulty;

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
                    if(0 <= globalX && globalX <= _size-1 && 0 <= globalZ && globalZ <= _size-1)
                        if(mode == 0)
                            if (MaxHeight > _mapVertices[globalX * (_size) + globalZ].y)
                                _mapVertices[globalX * (_size) + globalZ].y += heightmapOffset * Time.deltaTime;
                            else
                                _mapVertices[globalX * (_size) + globalZ].y = MaxHeight;
                        else if(mode == 1)
                            if(MinHeight < _mapVertices[globalX * (_size) + globalZ ].y)
                                _mapVertices[globalX * (_size) + globalZ ].y -= heightmapOffset*Time.deltaTime;
                            else
                                _mapVertices[globalX * (_size) + globalZ ].y = MinHeight;
                }
            }
            _meshFilter.mesh.vertices = _mapVertices;
            _meshCollider.sharedMesh.vertices = _mapVertices;
            _meshFilter.mesh.RecalculateNormals();
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
                        if (1 <= globalX && globalX <= _size -2 && 1 <= globalZ && globalZ <= _size -2)
                            if(mode==0)
                                _localDifficulty[globalX * (_size) + globalZ].r += heightmapOffset*Time.deltaTime/255f;
                            else if(mode==1)
                                _localDifficulty[globalX * (_size) + globalZ].r -= heightmapOffset*Time.deltaTime/255f;
                    }
                }
            }
            _meshFilter.mesh.colors = _localDifficulty;
        }

        public void UpdateMapProperties(float minHeight, float maxHeight)
        {
            MinHeight = minHeight;
            MaxHeight = maxHeight;
        }
     
        public void Init()
        {
            var generalMesh = new Mesh();

            generalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            _mapVertices = getVertices(_size);
            generalMesh.vertices = _mapVertices;
            generalMesh.triangles = getIndices(_size);
            generalMesh.uv = getUv(_size);
        
            _localDifficulty = getDifficulty(_size);
            generalMesh.colors = _localDifficulty;

            generalMesh.RecalculateNormals();
        
            _meshFilter.mesh = generalMesh;
            _meshCollider.sharedMesh = generalMesh;
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
}
