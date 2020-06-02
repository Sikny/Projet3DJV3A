using System;
using System.Collections;
using System.Collections.Generic;
using AStar;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public class test_worldgen : MonoBehaviour
{

    public int SIZE = 100;
    public Graph graph;
    private Random rand = new Random();
    public Algorithm alg;
    private Stack<Tile> itineraire;
    public Tile[,] tiles;
    private Vector3[] mapVertices;
    
    void Start()
    {
        //Rule r = Rule.readLevel("test");

        mapVertices = new Vector3[SIZE*SIZE];//r.loadHeightmap(getVertices(SIZE), SIZE);
        tiles = new Tile[SIZE, SIZE];
        
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                float y = 0;//mapVertices[i + j * SIZE].y;
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(i,y,j);
                MeshRenderer meshRenderer = cube.GetComponent<MeshRenderer>();
                
                meshRenderer.material.SetColor("_BaseColor", (i + j) % 2 == 0 ? Color.black : Color.white);
                if (y > 1.0f)
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
        
        graph = new Graph(tiles, SIZE,SIZE,tiles[0,0], tiles[SIZE-1,SIZE-1]);
        alg = new Algorithm(graph);
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
