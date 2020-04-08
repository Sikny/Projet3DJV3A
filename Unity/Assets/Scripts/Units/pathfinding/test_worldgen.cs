using System.Collections;
using System.Collections.Generic;
using AStar;
using UnityEngine;

public class test_worldgen : MonoBehaviour
{

    public int x=20, y=20;
    private Graph graph;
    void Start()
    {
        Tile[,] tiles = new Tile[x,y];
        
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(i,0,j);
                MeshRenderer meshRenderer = cube.GetComponent<MeshRenderer>();
                if (x / 2 == i && j != x/2)
                {
                    tiles[i, j] = new Tile(TileType.Wall, i, j);
                    meshRenderer.material.SetColor("_BaseColor", Color.red);
                }
                else
                {
                    tiles[i, j] = new Tile(TileType.Grass, i, j);
                    meshRenderer.material.SetColor("_BaseColor", (i + j) % 2 == 0 ? Color.black : Color.white);
                }
                cube.transform.SetParent(gameObject.transform);
            }
        }
        
        graph = new Graph(tiles, x,y,tiles[0,0], tiles[x-1,y-1]);
        Algorithm alg = new Algorithm(graph);
        alg.Solve();
        graph.ReconstructPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
