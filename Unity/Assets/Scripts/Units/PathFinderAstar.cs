using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Grid = TerrainGeneration.Grid;
using Vector2 = UnityEngine.Vector2;

public class PathFinderAstar
{
    private static PathFinderAstar instance;

    private Grid gridObject;
    private List<Vector2> path;

    public List<Vector2> BuildPath(Vector2 startPoint, Vector2 endPoint, int isWalkable)
    {
        gridObject = Grid.GetInstance();
        
        Debug.Log("start point:" + startPoint);
        Debug.Log("end point:" + endPoint);
        Debug.Log("isWalkable" + isWalkable);
        
        if (isWalkable == 0)
        {
            return path;
        }

        List<Vector2> openPaths = new List<Vector2>();
        List<Vector2> closedPaths = new List<Vector2>();
        List<Vector2> adjacents = new List<Vector2>();
        Dictionary<Vector2, Vector2> parentPath = new Dictionary<Vector2, Vector2>();
        openPaths.Append(startPoint);

        while (openPaths.Count != 0 && isWalkable == 0)
        {
            //Vector2 parent = currentPoint;
            Vector2 currentPoint = openPaths[0];
            openPaths.Remove(currentPoint);
            closedPaths.Add(currentPoint);
            adjacents = GetAdjacent((int)currentPoint.y, (int)currentPoint.x);
            
            
            foreach (Vector2 neighboor in adjacents)
            {
                bool walkable = gridObject.GridArray[(int)currentPoint.x, (int)currentPoint.y] == 0 ? true : false;
                
                if (!closedPaths.Contains(neighboor) && walkable)
                {
                    if (!openPaths.Contains(neighboor))
                    {
                        //Tile tile = new Tile(currentPoint,-1, 1);
                        parentPath.Add(neighboor,currentPoint);
   
                    }
                }
            }
        }

        return path;
    }
    
    public List<Vector2>GetAdjacent(int centerZ, int centerX)
    {
        List<Vector2> adjacents = new List<Vector2>();

        int maxZ = Grid.GetInstance().GridArray.GetLength(0);
        int maxX = Grid.GetInstance().GridArray.GetLength(1);
        for (int i = -3/2; i <= 3/2; i++)
        {
            /*if((centerX-Mathf.Abs(i) < 0 && i < 0) || (centerX+Mathf.Abs(i) >= maxX && i > 0))
                continue;*/
            for (int j = -3/2; j <= 3/2; j++)
            {
                /*if ((centerZ - Mathf.Abs(j) < 0 && j < 0)|| (centerZ + Mathf.Abs(j) >= maxZ && j > 0))
                    continue;*/

                adjacents.Add(new Vector2(centerZ + j, centerX + i));
            }
        }

        return adjacents;
    }
    
    public static PathFinderAstar GetInstance()
    {
        if(instance == null)
            instance = new PathFinderAstar();
        
        return instance;
    }
        
    

}
/*
public class Tile()
{
    Vector2 parent;
    int distanceToTarget = -1;
    int cost = 1;

    public Tile(Vector2 parent, int distanceToTarget, int cost)
    {
        this.parent = parent;
        this.distanceToTarget = distanceToTarget;
        this.cost = cost; 
    }

    public Vector2 Parent
    {
        get => parent;
        set => parent = value;
    }

    public int DistanceToTarget
    {
        get => distanceToTarget;
        set => distanceToTarget = value;
    }

    public int Cost
    {
        get => cost;
        set => cost = value;
    }
}
*/