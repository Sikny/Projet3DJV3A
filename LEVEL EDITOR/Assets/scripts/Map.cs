using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public const int SIZE = 100;
    public MeshFilter meshFilter;
    private GameObject map;

     void Start()
    {
        init();
    }

    public void init()
    {
        var mesh = new Mesh();

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = getVertices(SIZE);
        mesh.triangles = getIndices(SIZE);

        meshFilter.mesh = mesh;
    }

    public Vector3[] getVertices(int size)
    {
        Vector3[] result = new Vector3[(size*2+1)*(size*2+1)];

        for(int i = 0;i <= size*2; i++)
        {
            for(int j = 0;j <= size*2; j++)
            {
                result[i*(size*2+1) + j] = new Vector3(i-size, Random.Range(0,2), j-size);
            }
        }

        return result;
    }
    public int[] getIndices(int size)
    {
        int[] result = new int[6 * (size*2) * (size*2)];
        int c = 0;
        for (int i = 0; i < size*2; i++)
        {
            for (int j = 0; j < size*2-1; j++)
            {
                result[c++] = (i * (size * 2-1) + j);
                result[c++] = (i * (size * 2 - 1) + j + 1);
                result[c++] = (i * (size * 2 - 1) + j + 1 + (size * 2 - 1));

                result[c++] = (i * (size * 2 - 1) + j);
                result[c++] = (i * (size * 2 - 1) + j + 1 + (size * 2 - 1));
                result[c++] = (i * (size * 2 - 1) + j + (size * 2 - 1));
            }
        }

        return result;
    }
}
