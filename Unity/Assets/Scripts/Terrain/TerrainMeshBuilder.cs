using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable Unity.PreferAddressByIdToGraphicsParams

namespace Terrain {
    public enum TerrainSide {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Front = 3,
        Right = 4,
        Back = 5
    }

    public class TerrainMeshBuilder : MonoBehaviour {
        public Material material;
        [Range(2, 256)] public int resolution;
        [SerializeField, HideInInspector] private List<GameObject> meshObjects;
        private MeshFilter[] _meshFilters;
        private MeshRenderer[] _meshRenderers;
        private static MeshCollider _meshCollider;

        public Gradient heightGradient;
        public int textureResolution = 150;

        public TerrainOptions terrainOptions;
        public Cursor cursor;
        public Transform waterObject;

        // Path-finding fields
        [HideInInspector] public int[][] grid;

        private void Clear() {
            int meshesCount = meshObjects.Count;
            for (int i = 0; i < meshesCount; i++) {
                DestroyImmediate(meshObjects[i]);
            }

            meshObjects.Clear();
        }

        [ContextMenu("Init")]
        private void InitMeshes() {
            StartCoroutine(Init(null));
        }

        public IEnumerator Init(Action action) {
            TerrainGrid.Height = terrainOptions.height;
            TerrainGrid.Width = terrainOptions.width;

            TerrainGrid.Instance.cursor = cursor;

            grid = new int[terrainOptions.width][];

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
            waterObject.transform.localScale = new Vector3(terrainOptions.width - 0.0001f,
                waterObject.localScale.y, terrainOptions.height - 0.0001f);
            yield return null;
            action();
        }

        private void BuildTerrain() {
            Clear();
            meshObjects = new List<GameObject>();

            var sides = Enum.GetValues(typeof(TerrainSide));

            _meshFilters = new MeshFilter[sides.Length];
            _meshRenderers = new MeshRenderer[sides.Length];

            foreach (TerrainSide terrainSide in sides) {
                GameObject meshObj = new GameObject("TerrainMesh" + Enum.GetName(typeof(TerrainSide), terrainSide));
                meshObj.layer = 8;    // Ground
                meshObj.transform.parent = transform;
                if (terrainSide == TerrainSide.Top) {
                    var terrainRaycaster = meshObj.AddComponent<TerrainRaycaster>();
                    terrainRaycaster.width = terrainOptions.width;
                    terrainRaycaster.height = terrainOptions.height;
                }

                MeshRenderer meshRenderer = meshObj.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = material;
                _meshRenderers[(int) terrainSide] = meshRenderer;

                MeshFilter meshFilter = meshObj.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = new Mesh();
                _meshFilters[(int) terrainSide] = meshFilter;

                BuildTerrainMesh(terrainSide);
                meshObjects.Add(meshObj);
            }

            _meshCollider = _meshFilters[(int) TerrainSide.Top].gameObject.AddComponent<MeshCollider>();
            _meshCollider.sharedMesh = _meshFilters[(int) TerrainSide.Top].sharedMesh;

            Texture2D texture = new Texture2D(textureResolution, 1);
            Color[] colours = new Color[textureResolution];
            for (int i = 0; i < textureResolution; i++) {
                colours[i] = heightGradient.Evaluate(i / (textureResolution - 1f));
            }

            for (int i = 0; i < terrainOptions.width; i++) {
                grid[i] = new int[terrainOptions.height];
                for (int j = 0; j < terrainOptions.height; j++)
                {
                    Vector3 vec = new Vector3(i-terrainOptions.width/2,0, j-terrainOptions.height/2);
                    //Vector2 percent = new Vector2() / ;
                    float h = CalculateHeight(vec);
                    if (h > 0.01f || h < -0.01f)
                        grid[i][j] = 1;
                    else
                        grid[i][j] = 0;
                }
            }

            texture.SetPixels(colours);
            texture.Apply();

            material.SetTexture("_terrainTexture", texture);
        }
        
        // TODO SIMPLIFY CODE
        private void BuildTerrainMesh(TerrainSide side) {
            int index = (int) side;
            Mesh mesh = _meshFilters[index].sharedMesh;

            Vector3[] vertices;
            int[] triangles;
            int triIndex = 0;

            int i;
            switch (side) {
                case TerrainSide.Top:
                    vertices = new Vector3[resolution * resolution];
                    triangles = new int[(resolution - 1) * (resolution - 1) * 6];

                    for (int y = 0; y < resolution; y++) {
                        for (int x = 0; x < resolution; x++) {
                            i = x + y * resolution;
                            Vector2 percent = new Vector2(x, y) / (resolution - 1);
                            Vector3 point = new Vector3((percent.x - .5f) * TerrainGrid.Width,
                                0f,
                                -(percent.y - .5f) * TerrainGrid.Height);
                            point.y = CalculateHeight(point);
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

                    break;
                case TerrainSide.Bottom:
                    vertices = new Vector3[resolution * resolution];
                    triangles = new int[(resolution - 1) * (resolution - 1) * 6];

                    for (int y = 0; y < resolution; y++) {
                        for (int x = 0; x < resolution; x++) {
                            i = x + y * resolution;
                            Vector2 percent = new Vector2(x, y) / (resolution - 1);
                            Vector3 point = new Vector3((percent.x - .5f) * TerrainGrid.Width,
                                -1f,
                                -(percent.y - .5f) * TerrainGrid.Height);
                            vertices[i] = point;

                            if (x != resolution - 1 && y != resolution - 1) {
                                triangles[triIndex] = i + resolution;
                                triangles[triIndex + 1] = i + resolution + 1;
                                triangles[triIndex + 2] = i;

                                triangles[triIndex + 3] = i + resolution + 1;
                                triangles[triIndex + 4] = i + 1;
                                triangles[triIndex + 5] = i;
                                triIndex += 6;
                            }
                        }
                    }

                    break;
                default:
                    vertices = new Vector3[resolution * 2];
                    triangles = new int[(resolution - 1) * 6];
                    float semiWidth = terrainOptions.width / 2f;
                    float semiHeight = terrainOptions.height / 2f;
                    i = 0;

                    switch (side) {
                        case TerrainSide.Back:
                            for (int x = 0; x < resolution; x++) {
                                for (int y = -1; y <= 0; y++) {
                                    float scaledX = (x / (resolution - 1f) - 0.5f) * TerrainGrid.Width;
                                    vertices[i] = new Vector3(scaledX, y, semiWidth);
                                    if (vertices[i].y > -1f) {
                                        vertices[i].y = CalculateHeight(vertices[i]);
                                    }

                                    if (x != resolution - 1 && y != 0) {
                                        triangles[triIndex] = i;
                                        triangles[triIndex + 1] = i + 2;
                                        triangles[triIndex + 2] = i + 1;

                                        triangles[triIndex + 3] = i + 1;
                                        triangles[triIndex + 4] = i + 2;
                                        triangles[triIndex + 5] = i + 3;
                                        triIndex += 6;
                                    }

                                    i++;
                                }
                            }

                            break;
                        case TerrainSide.Front:
                            for (int x = 0; x < resolution; x++) {
                                for (int y = -1; y <= 0; y++) {
                                    float scaledX = (x / (resolution - 1f) - 0.5f) * TerrainGrid.Width;
                                    vertices[i] = new Vector3(scaledX, y, -semiWidth);
                                    if (vertices[i].y > -1f) {
                                        vertices[i].y = CalculateHeight(vertices[i]);
                                    }

                                    if (x != resolution - 1 && y != 0) {
                                        triangles[triIndex] = i;
                                        triangles[triIndex + 1] = i + 1;
                                        triangles[triIndex + 2] = i + 2;

                                        triangles[triIndex + 3] = i + 1;
                                        triangles[triIndex + 4] = i + 3;
                                        triangles[triIndex + 5] = i + 2;
                                        triIndex += 6;
                                    }

                                    i++;
                                }
                            }

                            break;
                        case TerrainSide.Left:
                            for (int z = 0; z < resolution; z++) {
                                for (int y = -1; y <= 0; y++) {
                                    float scaledZ = (z / (resolution - 1f) - 0.5f) * TerrainGrid.Height;
                                    vertices[i] = new Vector3(-semiHeight, y, scaledZ);
                                    if (vertices[i].y > -1f) {
                                        vertices[i].y = CalculateHeight(vertices[i]);
                                    }

                                    if (y != 0 && z != resolution - 1) {
                                        triangles[triIndex] = i;
                                        triangles[triIndex + 1] = i + 2;
                                        triangles[triIndex + 2] = i + 1;

                                        triangles[triIndex + 3] = i + 1;
                                        triangles[triIndex + 4] = i + 2;
                                        triangles[triIndex + 5] = i + 3;
                                        triIndex += 6;
                                    }

                                    i++;
                                }
                            }

                            break;
                        case TerrainSide.Right:
                            for (int z = 0; z < resolution; z++) {
                                for (int y = -1; y <= 0; y++) {
                                    float scaledZ = (z / (resolution - 1f) - 0.5f) * TerrainGrid.Height;
                                    vertices[i] = new Vector3(semiHeight, y, scaledZ);
                                    if (vertices[i].y > -1f) {
                                        vertices[i].y = CalculateHeight(vertices[i]);
                                    }

                                    if (y != 0 && z != resolution - 1) {
                                        triangles[triIndex] = i;
                                        triangles[triIndex + 1] = i + 1;
                                        triangles[triIndex + 2] = i + 2;

                                        triangles[triIndex + 3] = i + 1;
                                        triangles[triIndex + 4] = i + 3;
                                        triangles[triIndex + 5] = i + 2;
                                        triIndex += 6;
                                    }

                                    i++;
                                }
                            }

                            break;
                    }

                    int verticesCount = vertices.Length;
                    for (i = 0; i < verticesCount; i++)
                        if (vertices[i].y > 0.01f)
                            vertices[i].y = CalculateHeight(vertices[i]);
                    break;
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        private float CalculateHeight(Vector3 vertex) {
            float result = 0;
            Vector2Int intVec = new Vector2Int((int) (vertex.x+0.5f), (int) (vertex.z+0.5f));
            if (terrainOptions.modifierHeightMap.ContainsKey(intVec)) {
                result = terrainOptions.modifierHeightMap[intVec];
            }

            foreach (var waterValue in _waterData) {
                if (waterValue.Contains(intVec.x, intVec.y)) {
                    result = -0.5f;
                }
            }

            return result;
        }

        private readonly List<UnitList> _waterData = new List<UnitList>();

        private void BuildOneWaterArea() {
            Vector2Int tmp;
            int randomX, randomY;
            var waterList = new UnitList();
            while (waterList.Count() < terrainOptions.maxWaterSize) {
                if (waterList.Count() == 0) {
                    randomY = Random.Range(-terrainOptions.height / 2, terrainOptions.height / 2);
                    randomX = Random.Range(-terrainOptions.width / 2, terrainOptions.width / 2);
                }
                else {
                    tmp = waterList.Last();
                    randomY = Random.Range(tmp.y - 1, tmp.y + 2); // +2 because exclusive
                    randomX = Random.Range(tmp.x - 1, tmp.y + 2);
                }

                if (randomX < -terrainOptions.width / 2 || randomY < -terrainOptions.height / 2 ||
                    randomX >= terrainOptions.width / 2 || randomY >= terrainOptions.height / 2
                    || !waterList.HasNeighbour(randomX, randomY))
                    continue;
                waterList.Add(randomX, randomY);
            }

            _waterData.Add(waterList);
        }

        // TODO : ROUND MOUNTAINS ??
        private void BuildOneMountain() {
            int posX = Random.Range(-terrainOptions.width / 2, terrainOptions.width / 2);
            int posZ = Random.Range(-terrainOptions.height / 2, terrainOptions.height / 2);
            Vector2 mountainSource = new Vector2(posX, posZ);
            int mountTipHeight = Random.Range(terrainOptions.minMountainHeight, terrainOptions.maxMountainHeight + 1);
            if (!terrainOptions.modifierHeightMap.ContainsKey(mountainSource))
                terrainOptions.modifierHeightMap.Add(mountainSource, mountTipHeight);
            int radius = 1;
            for (int i = mountTipHeight - 1; i > 0; i--) {
                SetMountainFloor(i, mountainSource, radius++);
            }
        }

        private void SetMountainFloor(int yPos, Vector2 center, int radius) {
            int y = yPos;
            for (float x = center.x - radius; x <= center.x + radius; x++) {
                for (float z = center.y - radius; z <= center.y + radius; z++) {
                    if (Random.Range(0f, 1f) > 0.6f) y = yPos - 1;
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