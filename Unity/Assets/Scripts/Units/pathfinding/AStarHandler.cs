using System.Collections.Generic;
using Terrain;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Units.PathFinding {
    public class AStarHandler
    {
        public int[][] grid;
        private readonly int _gridSize;

        private NativeArray<float> _costMatrix;
        private NativeArray<float> _heuristicMatrix;

        private List<Vector3> _wayPoints;
        
        public AStarHandler(TerrainOptions terrainOptions, TerrainMeshBuilder terrainMesh)
        {
            _gridSize = terrainOptions.width;
            _wayPoints = new List<Vector3>();

            _wayPoints = new List<Vector3>();

            grid = terrainMesh.grid;

            string strDebug = "";
            for (int i = 0; i < _gridSize; i++) {
                for (int j = 0; j < _gridSize; j++)
                    strDebug += grid[i][j];
                strDebug += "\n";
            }
            Debug.Log(strDebug);
            
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
        
        private int PosToId(Vector3 pos) {
            return Mathf.RoundToInt(pos.z) * _gridSize + Mathf.RoundToInt(pos.x);
        }

        public void UpdateTransform(AbstractUnit unitToMove, Vector3 destination, float unitSpeed)
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
                var nodePos = new Vector3(node % _gridSize, targetTransform.position.y, Mathf.FloorToInt(node / (float) _gridSize));
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
                    unitToMove.SetPosition(unitToMove.transform.position);

                    //_targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
                }
            }

            /*targetTransform.rotation =
                Quaternion.RotateTowards(targetTransform.rotation, _targetRotation, rotateSpeed - Time.deltaTime);*/
        }
    }
}