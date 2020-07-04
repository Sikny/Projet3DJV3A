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
            StartCoroutine(terrainMesh.Init(InitAStar, null));
        }

        private void InitAStar() {
            aStarHandler.Init(terrainOptions);
            /*string debugStr = "";
            for (int i = 0; i < aStarHandler.Grid.Length; i++) {
                for (int j = 0; j < aStarHandler.Grid[i].Length; j++)
                    debugStr += aStarHandler.Grid[i][j];
                debugStr += "\n";
            }*/
            var offset = Vector3.right * (TerrainGrid.Width / 2f) +
                         Vector3.forward * (TerrainGrid.Height / 2f);
            terrainMesh.transform.position += offset;
            Camera.main.transform.position += offset;
        }

        private void Update() {
            if (Input.GetMouseButtonUp(0)) {
                aStarEntity.MoveTo(terrainMesh.cursor.transform.position, aStarHandler);
            }
        }
    }
}
