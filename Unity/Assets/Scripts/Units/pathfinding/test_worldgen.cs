using System;
using System.Collections;
using System.Collections.Generic;
using AStar;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public class test_worldgen : MonoBehaviour
{

    public int x=20, y=20;
    public Graph graph;
    private Random rand = new Random();
    public Algorithm alg;
    private Stack<Tile> itineraire;
    public Tile[,] tiles;
    void Start()
    {
        tiles = new Tile[x, y];
        
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float y = Mathf.PerlinNoise(i/5f, j/5f) * 3f;
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(i,y,j);
                MeshRenderer meshRenderer = cube.GetComponent<MeshRenderer>();
                
                meshRenderer.material.SetColor("_BaseColor", (i + j) % 2 == 0 ? Color.black : Color.white);
                if (y < 1.0f)
                {
                    tiles[i, j] = new Tile(TileType.Wall, i, j, cube.transform.position); //TODO : y should be the normal of neightborg blocks
                    meshRenderer.material.SetColor("_BaseColor", Color.blue);
                }
                else
                {
                    tiles[i, j] = new Tile(TileType.Pente, i, j, cube.transform.position);
                    meshRenderer.material.SetColor("_BaseColor", (i + j) % 2 == 0 ? Color.black : Color.white);
                }
                cube.transform.SetParent(gameObject.transform);
            }
        }
        
        graph = new Graph(tiles, x,y,tiles[0,0], tiles[x-1,y-1]);
        alg = new Algorithm(graph);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
