using System.Collections.Generic;
using Items;
using Terrain;
using Units.PathFinding;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Utility;

namespace Units {
    public class SystemUnit : MonoBehaviour {
        [SerializeField] private int sizeUnit = 9;

        public EntityDict entityDict;
        public PlayerUnit playerUnitPrefab;
        public AiUnit aiUnitPrefab;
        private List<AbstractUnit> _units = new List<AbstractUnit>();


        /** Données de l'ancien système nécessaire aux unités*/
        public Camera cam;
        public LayerMask groundMask;
        public float rotationSpeed = 300f;
        public float speed = 5f;
        
        public const float YPos = 0.5f;

        public bool isRunning;

        public void SetRunning(bool run)
        {
            isRunning = run;
        }
        
        public void Start() {
            UnitLibData.selectedUnit = null;

            /* On se servira de ça pour appeler les updates des units */
            UnitLibData.cam = cam;
            UnitLibData.speed = speed;
            UnitLibData.rotationSpeed = rotationSpeed;
            UnitLibData.groundMask = groundMask;
            UnitLibData.units = _units; // Penser à update si 
            UnitLibData.deltaTime = 0;
        }

        #region ASTAR
        private TerrainMeshBuilder _terrainMeshBuilder;
        public static Collider groundCollider;
        public static int gridSize;
        public static int[][] grid;
        private static List<Vector3> _wayPoints;
        private static NativeArray<float> _costMatrix;
        private static NativeArray<float> _heuristicMatrix;
        private static Quaternion _targetRotation;

        public Vector3 TerrainOffset()
        {
            return Vector3.right * (TerrainGrid.Width / 2f) +
                Vector3.forward * (TerrainGrid.Height / 2f);
        }
        
        public void InitPathFinding()
        {
            _terrainMeshBuilder = FindObjectOfType<TerrainMeshBuilder>();
            _terrainMeshBuilder.transform.position += TerrainOffset();
            cam.transform.position += TerrainOffset();
            gridSize = _terrainMeshBuilder.terrainOptions.width;
            _wayPoints = new List<Vector3>();

            groundCollider = TerrainMeshBuilder.meshCollider;

            _wayPoints = new List<Vector3>();

            grid = _terrainMeshBuilder.grid;

            string strDebug = "";
            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++)
                    strDebug += grid[i][j];
                strDebug += "\n";
            }
            print(strDebug);
            
            var linesCount = grid.Length;
            var colsCount = grid[0].Length;
            var costMatrixWidth = linesCount * colsCount;

            _costMatrix = new NativeArray<float>(costMatrixWidth * costMatrixWidth, Allocator.Persistent);
            _heuristicMatrix = new NativeArray<float>(costMatrixWidth, Allocator.Persistent);

            for (var i = 0; i < linesCount; i++) {
                for (var j = 0; j < colsCount; j++) {
                    var sourceIdx = i * colsCount + j;

                    for (var i1 = 0; i1 < linesCount; i1++) {
                        for (var j1 = 0; j1 < colsCount; j1++) {
                            var targetIdx = i1 * colsCount + j1;

                            if (grid[i][j] != 1 && grid[i1][j1] != 1 && (i == i1 && Mathf.Abs(j - j1) == 1 ||
                                                                         j == j1 && Mathf.Abs(i - i1) == 1)) {
                                _costMatrix[sourceIdx * costMatrixWidth + targetIdx] = 1;
                            }
                            else {
                                _costMatrix[sourceIdx * costMatrixWidth + targetIdx] = float.MaxValue;
                            }
                        }
                    }
                }
            }
        }
        
        private static int PosToId(Vector3 pos) {
            return Mathf.RoundToInt(pos.z) * gridSize + Mathf.RoundToInt(pos.x);
        }

        private void OnDestroy() {
            _costMatrix.Dispose();
            _heuristicMatrix.Dispose();
        }
        
        public static void UpdateTransform(AbstractUnit unitToMove, Vector3 destination, float unitSpeed)
        {
            Transform targetTransform = unitToMove.transform;
            var linesCount = grid.Length;
            var colsCount = grid[0].Length;
            var costMatrixWidth = linesCount * colsCount;

            //Update Heuristic Matrix to target node
            for (var i = 0; i < linesCount; i++) {
                for (var j = 0; j < colsCount; j++) {
                    var sourceIdx = i * colsCount + j;

                    _heuristicMatrix[sourceIdx] = Mathf.Abs(i - Mathf.RoundToInt(destination.z)) +
                                                 Mathf.Abs(j - Mathf.RoundToInt(destination.x));
                }
            }
            var position = targetTransform.position;
            var job = new PathFindingJob {
                costMatrix = _costMatrix,
                nodesCount = costMatrixWidth,
                heuristicMatrix = _heuristicMatrix,
                startNodeId = PosToId(position),
                endNodeId = PosToId(destination),
                bestCost = new NativeArray<float>(1, Allocator.TempJob),
                exploredNodesCount = new NativeArray<int>(1, Allocator.TempJob),
                bestPath = new NativeList<int>(Allocator.TempJob)
            };
            
            var handler = job.Schedule();

            handler.Complete();
            
            _wayPoints.Clear();
            for (var n = 1; n < job.bestPath.Length; n++) {
                var node = job.bestPath[n];
                var nodePos = new Vector3(node % gridSize, targetTransform.position.y, Mathf.FloorToInt(node / (float) gridSize));
                _wayPoints.Add(nodePos);
            }

            job.exploredNodesCount.Dispose();
            job.bestCost.Dispose();
            job.bestPath.Dispose();

                // Show Debug Path
            var lastPoint = targetTransform.position;
            for (var i = 0; i < _wayPoints.Count; i++) {
                var point = _wayPoints[i];
                Debug.DrawLine(lastPoint, point, Color.green, 0.1f);

                lastPoint = point;
            }

            // Move Target
            if (_wayPoints.Count > 0) {
                var point = _wayPoints[0];
                position = targetTransform.position;
                var vectorToTarget = (point - position);
                var distanceToTarget = vectorToTarget.magnitude;

                if (distanceToTarget <= Time.deltaTime * unitSpeed) {
                    targetTransform.position = point;
                    _wayPoints.RemoveAt(0);
                }
                else {
                    targetTransform.position += Time.deltaTime * unitSpeed * UnitLibData.speed * vectorToTarget / distanceToTarget;

                    //_targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
                }
            }

            /*targetTransform.rotation =
                Quaternion.RotateTowards(targetTransform.rotation, _targetRotation, rotateSpeed - Time.deltaTime);*/
        }
        #endregion

        public Transform SpawnUnit(EntityType unitType, AbstractUnit unit, Vector3 position) {
            var newUnit = Instantiate(unit);
            newUnit.SetPosition(position);
            newUnit.Init(unitType, entityDict.GetEntityType(unitType), sizeUnit);
            _units.Add(newUnit);
            UnitLibData.units = _units;
            return newUnit.transform;
        }
        
        public void DoClick() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
           
            // Placement d'une unité de l'inventaire
            if (GameSingleton.Instance.uiManager.inventory.selectedStoreUnit != null
                && Physics.Raycast(ray, out hit, 100f, 1 << 8)) {
                Vector3 position = new Vector3(Mathf.Floor(hit.point.x)+0.5f, YPos,
                    Mathf.Floor(hit.point.z)+0.5f) ;
                StoreUnit unit = GameSingleton.Instance.uiManager.inventory.selectedStoreUnit;
                SpawnUnit(unit.entityType, playerUnitPrefab, position);
                GameSingleton.Instance.uiManager.inventory.RemoveUnit(unit);
                GameSingleton.Instance.uiManager.inventory.selectedStoreUnit = null;
            }
            
            // Fight start
            if (!isRunning) return;
            // Allied Unit selection
            if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
            {
                UnitLibData.selectedUnit = hit.transform.GetComponent<PlayerUnit>();
            }
            else if (UnitLibData.selectedUnit != null)
            {
                // Click on ground
                if (Physics.Raycast(ray, out hit, 100f, 1 << 8))
                {
                    UnitLibData.selectedUnit.SetTargetPosition(TerrainGrid.Instance.cursor.transform.position);
                }
            }
        }

        public void Update() {
            if (isRunning)
            {
                UnitLibData.deltaTime = Time.deltaTime;
                int unitCount = _units.Count;
                for (int i = 0; i < unitCount; i++)
                {
                    _units[i].UpdateUnit();
                    if (_units[i].GetNumberAlive() <= 0)
                    {
                        _units[i].Kill();
                        _units.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}