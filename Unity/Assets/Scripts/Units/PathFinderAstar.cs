using System.Collections.Generic;
using System.Linq;
using TerrainGeneration;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PathFinderAstar {
    private static PathFinderAstar instance;

    private List<Vector2> path;
    public List<Vector2> BuildPath(Vector2 startPoint, Vector2 endPoint, int isWalkable) {
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
                bool walkable = TerrainGrid.Instance.GridArray[(int)currentPoint.x, (int)currentPoint.y] == 0 ? true : false;
                
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

        for (int i = -3/2; i <= 3/2; i++) {
            for (int j = -3/2; j <= 3/2; j++) {
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