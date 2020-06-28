using Terrain;
using Units.Pathfinding;
using Units.PathFinding;
using UnityEngine;

namespace Scenes.AStarTests {
    public class Initializer : MonoBehaviour {
        [SerializeField] private TerrainMeshBuilder terrainMesh;
        [SerializeField] private TerrainOptions terrainOptions;
        [SerializeField] private AStarHandler aStarHandler;
        [SerializeField] private AStarEntity aStarEntity;

        private void Awake() {
            Debug.Log("Building Terrain");
            StartCoroutine(terrainMesh.Init(InitAStar));
        }

        private void InitAStar() {
            Debug.Log("Initializing AStar");
            aStarHandler.Init(terrainOptions);
            string debugStr = "";
            for (int i = 0; i < aStarHandler.Grid.Length; i++) {
                for (int j = 0; j < aStarHandler.Grid[i].Length; j++)
                    debugStr += aStarHandler.Grid[i][j];
                debugStr += "\n";
            }
            Debug.Log("Generated grid: " + debugStr);
        }

        private void Update() {
            if (Input.GetMouseButtonUp(0)) {
                Debug.Log("Moving to " + terrainMesh.cursor.transform.position);
                aStarEntity.MoveTo(terrainMesh.cursor.transform.position, aStarHandler);
            }
        }
    }
}
