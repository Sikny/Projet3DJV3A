using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable Unity.PreferAddressByIdToGraphicsParams

namespace TerrainGeneration {
    public class TerrainMeshDeformation : MonoBehaviour {
        public Material material;
        [Range(2, 256)] public int resolution;
        [SerializeField, HideInInspector] private MeshFilter meshFilter;
        [SerializeField, HideInInspector] private MeshRenderer meshRenderer;

        private float _toleranceX;
        private float _toleranceY;
        
        
        private MinMax _minMax;
        public Gradient heightGradient;
        public int textureResolution = 150;
        
        public TerrainOptions terrainOptions;
        public Transform cursor;
        
        private void Awake() {
            Init();
        }

        [ContextMenu("Build")]
        private void Init() {
            _toleranceX = (float) terrainOptions.height/resolution;
            _toleranceY = (float) terrainOptions.width/resolution;
            TerrainGrid.Height = terrainOptions.height;
            TerrainGrid.Width = terrainOptions.width;

            TerrainGrid.Instance.cursor = cursor;
            
            _waterData.Clear();
            
            Random.InitState(terrainOptions.rules.seedWorld);
            // build water areas
            for (int i = 0; i < terrainOptions.waterCount; i++) {
                BuildOneWaterArea();
            }

            terrainOptions.modifierHeightMap.Clear();
            
            // build mountains
            for (int i = 0; i < terrainOptions.mountainCount; i++) {
                BuildOneMountain();
            }

            BuildTerrain();
        }
        
        private void BuildTerrain() {
            terrainOptions.rules.seedWorld = Random.Range(0, 100000);
            
            if (meshFilter == null) {
                GameObject meshObject = new GameObject("Mesh");
                meshObject.transform.parent = transform;
                meshRenderer = meshObject.AddComponent<MeshRenderer>();

                meshFilter = meshObject.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = new Mesh();
            }

            BuildMesh();

            Texture2D texture = new Texture2D(textureResolution, 1);
            Color[] colours = new Color[textureResolution];
            for (int i = 0; i < textureResolution; i++) {
                colours[i] = heightGradient.Evaluate(i / (textureResolution-1f));
            }

            texture.SetPixels(colours);
            texture.Apply();

            material.SetVector("_YMinMax", new Vector4(_minMax.Min, _minMax.Max));
            material.SetTexture("_terrainTexture", texture);

            meshRenderer.sharedMaterial = material;
        }

        private void BuildMesh() {
            _minMax = new MinMax();
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = new Vector3[resolution * resolution];
            int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
            int triIndex = 0;

            for (int y = 0; y < resolution; y++) {
                for (int x = 0; x < resolution; x++) {
                    int i = x + y * resolution;
                    Vector2 percent = new Vector2(x, y) / (resolution - 1);
                    Vector3 point = new Vector3((percent.x - .5f) * TerrainGrid.Width, 
                        0f,
                        -(percent.y - .5f) * TerrainGrid.Height);
                    point.y = CalculateHeight(point);
                    _minMax.HandleValue(point.y);
                    vertices[i] = point;

                    if (x != resolution - 1 && y != resolution - 1) {
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + resolution + 1;
                        triangles[triIndex + 2] = i + resolution;

                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + resolution + 1;
                        triIndex += 6;
                    }

                }
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        private float CalculateHeight(Vector3 vertex) {
            Vector2Int intVec = new Vector2Int((int)vertex.x, (int)vertex.z);
            if (terrainOptions.modifierHeightMap.ContainsKey(intVec)) {
                return terrainOptions.modifierHeightMap[intVec];
            }

            foreach (var waterValue in _waterData) {
                if (waterValue.Contains(intVec.x, intVec.y)) {
                    return -0.5f;
                }
            }
            return 0;
        }

        private List<UnitList> _waterData = new List<UnitList>();
        private void BuildOneWaterArea() {
            Vector2Int tmp;
            int randomX, randomY;
            var waterList = new UnitList();
            int crashHandler = 0;
            while (waterList.Count() < terrainOptions.maxWaterSize 
                   && crashHandler < terrainOptions.crashLoopLimit) {
                crashHandler++;
                if (waterList.Count() == 0) {
                    // Horizontal
                    randomY = Random.Range(0, terrainOptions.width);
                    // Vertical
                    randomX = Random.Range(0, terrainOptions.height);
                    if (randomX > 0 && randomX < terrainOptions.width - 1 
                                    && randomY > 0 && randomY < terrainOptions.height - 1)
                        continue;
                } else {
                    tmp = waterList.Last();
                    randomY = Random.Range(tmp.y - 1, tmp.y + 2);
                    randomX = Random.Range(tmp.x - 1, tmp.y + 2);
                }
                if (waterList.Contains(randomX, randomY) || randomX < 0 || randomY < 0
                    || randomX >= terrainOptions.width || randomY >= terrainOptions.height
                    || !waterList.HasNeighbour(randomX, randomY))
                    continue;
                waterList.Add(randomX, randomY);
            }
            _waterData.Add(waterList);
        }
        
        private void BuildOneMountain() {
            int posX = Random.Range(-terrainOptions.width/2, terrainOptions.width/2);
            int posZ = Random.Range(-terrainOptions.height/2, terrainOptions.height/2);
            Vector2 mountainSource = new Vector2(posX, posZ);
            int mountTipHeight = Random.Range(terrainOptions.minMountainHeight, terrainOptions.maxMountainHeight + 1);
            if(!terrainOptions.modifierHeightMap.ContainsKey(mountainSource))
                terrainOptions.modifierHeightMap.Add(mountainSource, mountTipHeight);
            int radius = 1;
            for (int i = mountTipHeight-1; i > 0; i--) {
                SetMountainFloor(i, mountainSource, radius++);
            }
        }
        
        private void SetMountainFloor(int yPos, Vector2 center, int radius) {
            int y = yPos;
            for (float x = center.x - radius; x <= center.x + radius; x++) {
                for (float z = center.y - radius; z <= center.y + radius; z++) {
                    if (Random.Range(0f, 1f) > 0.6f) y = yPos-1;
                    Vector2 currentUnit = new Vector2(x, z);
                    if (!terrainOptions.modifierHeightMap.ContainsKey(currentUnit)) {
                        terrainOptions.modifierHeightMap.Add(currentUnit, y);
                    }
                    y = yPos;
                }
            }
        }
    }
}