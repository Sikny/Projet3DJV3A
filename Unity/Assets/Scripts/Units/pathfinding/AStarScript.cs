using System.Collections.Generic;
using Terrain;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Units.PathFinding {
    public class AStarScriptun : MonoBehaviour {
        public Camera targetCamera;
        public Transform targetTransform;
        public float targetSpeed;
        public float rotateSpeed;
        public Collider groundCollider;

        public int gridSize;

        public int[][] grid;

        private NativeArray<float> _costMatrix;
        private NativeArray<float> _heuristicMatrix;
        private List<Vector3> _wayPoints;
        private Quaternion _targetRotation;

        [SerializeField] private TerrainMeshBuilder terrainMeshBuilder;

        private void Awake() {
            StartCoroutine(terrainMeshBuilder.Init(null));
        }
        
        public void Init()
        {
            
        }

        private void UpdateTransform(Transform t) {
            if (Input.GetMouseButtonUp(0) &&
                groundCollider.Raycast(targetCamera.ScreenPointToRay(Input.mousePosition), out _, 1000f)) {
                var linesCount = grid.Length;
                var colsCount = grid[0].Length;
                var costMatrixWidth = linesCount * colsCount;

                Vector3 cursorPos = terrainMeshBuilder.cursor.transform.position;

                //Update Heuristic Matrix to target node
                for (var i = 0; i < linesCount; i++) {
                    for (var j = 0; j < colsCount; j++) {
                        var sourceIdx = i * colsCount + j;

                        _heuristicMatrix[sourceIdx] = Mathf.Abs(i - Mathf.RoundToInt(cursorPos.z)) +
                                                     Mathf.Abs(j - Mathf.RoundToInt(cursorPos.x));
                    }
                }
                var position = targetTransform.position;
                Debug.DrawLine(position, position+Vector3.up, Color.red, 5f);
                Debug.DrawLine(cursorPos, cursorPos+Vector3.up, Color.blue, 5f);
                var job = new PathFindingJob {
                    costMatrix = _costMatrix,
                    nodesCount = costMatrixWidth,
                    heuristicMatrix = _heuristicMatrix,
                    startNodeId = PosToId(position),
                    endNodeId = PosToId(cursorPos),
                    bestCost = new NativeArray<float>(1, Allocator.TempJob),
                    exploredNodesCount = new NativeArray<int>(1, Allocator.TempJob),
                    bestPath = new NativeList<int>(Allocator.TempJob)
                };

                //var sw = new Stopwatch();
                //sw.Start();
                var handler = job.Schedule();

                handler.Complete();
                /*sw.Stop();
                Debug.Log($"Job Execution in {sw.ElapsedMilliseconds}ms");
                Debug.Log($"Explored Nodes Count : {job.exploredNodesCount[0]}");
                Debug.Log($"Best Path Cost : {job.bestCost[0]}");*/

                //var sb = new StringBuilder();
                _wayPoints.Clear();
                for (var n = 1; n < job.bestPath.Length; n++) {
                    var node = job.bestPath[n];
                    var nodePos = new Vector3(node % gridSize, 1f, Mathf.FloorToInt(node / (float) gridSize));
                    //sb.Append($"{node}, ");
                    _wayPoints.Add(nodePos);
                }

                //Debug.Log($"Best Path : {string.Join(", ", job.bestPath.ToArray())}");

                job.exploredNodesCount.Dispose();
                job.bestCost.Dispose();
                job.bestPath.Dispose();
            }

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
                var position = targetTransform.position;
                var vectorToTarget = (point - position);
                var distanceToTarget = vectorToTarget.magnitude;

                if (distanceToTarget <= Time.deltaTime * targetSpeed) {
                    targetTransform.position = point;
                    _wayPoints.RemoveAt(0);
                }
                else {
                    targetTransform.position += Time.deltaTime * targetSpeed * vectorToTarget / distanceToTarget;

                    _targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
                }
            }

            targetTransform.rotation =
                Quaternion.RotateTowards(targetTransform.rotation, _targetRotation, rotateSpeed - Time.deltaTime);
        }

        private int PosToId(Vector3 pos) {
            return Mathf.RoundToInt(pos.z) * gridSize + Mathf.RoundToInt(pos.x);
        }

        private void OnDestroy() {
            _costMatrix.Dispose();
            _heuristicMatrix.Dispose();
        }
    }
}