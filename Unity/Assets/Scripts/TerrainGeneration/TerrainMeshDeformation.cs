using System;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable Unity.PreferAddressByIdToGraphicsParams

namespace TerrainGeneration {
    public class TerrainMeshDeformation : MonoBehaviour {
        public Material material;
        public int seed;
        [Range(2, 256)] public int resolution;
        [SerializeField, HideInInspector] private MeshFilter meshFilter;
        [SerializeField, HideInInspector] private MeshRenderer meshRenderer;

        private MinMax _minMax;
        public Gradient heightGradient;
        public int textureResolution = 150;

        private void Awake() {
            BuildTerrain();
        }

        [ContextMenu("Build")]
        private void BuildTerrain() {
            seed = Random.Range(0, 100000);
            if (meshFilter == null) {
                GameObject meshObject = new GameObject("Mesh");
                meshObject.transform.parent = transform;
                meshRenderer = meshObject.AddComponent<MeshRenderer>();

                meshFilter = meshObject.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = new Mesh();
            }
            BuildMesh();
            
            Texture2D texture = new Texture2D(textureResolution,1);
            Color[] colours = new Color[textureResolution];
            for (int i = 0; i < textureResolution; i++) {
                colours[i] = heightGradient.Evaluate(i / (textureResolution - 1f));
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

            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int i = x + y * resolution;
                    Vector2 percent = new Vector2(x, y) / (resolution - 1);
                    Vector3 pointOnUnitCube = Mathf.PerlinNoise((percent.x-0.5f)*20 + seed, (percent.y-0.5f)*20 + seed) * Vector3.up + (percent.x - .5f) * 20 * Vector3.right + (percent.y - .5f) * 20 * Vector3.back;
                    
                    vertices[i] = pointOnUnitCube;

                    if (x != resolution - 1 && y != resolution - 1)
                    {
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + resolution + 1;
                        triangles[triIndex + 2] = i + resolution;

                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + resolution + 1;
                        triIndex += 6;
                    }
                    _minMax.HandleValue(pointOnUnitCube.y);
                }
            }
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }
    }
}
