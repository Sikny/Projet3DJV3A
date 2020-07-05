using System;
using System.Collections.Generic;
using Terrain;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Units.PathFinding {
    public class AStarHandler : MonoBehaviour {
        [SerializeField] private Transform terrainStart;
        [SerializeField] private Transform terrainEnd;
        
        private int[][] _grid;
        private int _gridSize;

        private NativeArray<float> _costMatrix;
        private NativeArray<float> _heuristicMatrix;

        private List<Vector3> _wayPoints;

        public void Init(TerrainOptions terrainOptions) {
            _gridSize = terrainOptions.width;
            
            // Terrain limits
            Vector3 halfVec = Vector3.one / 2;
            halfVec.y = 0f;
            terrainStart.position = - Vector3.right * terrainOptions.width/2 - Vector3.forward * terrainOptions.height/2 + halfVec;
            terrainEnd.position = Vector3.right * terrainOptions.width/2 + Vector3.forward * terrainOptions.height/2 - halfVec;
            float castHeight = terrainOptions.maxMountainHeight+2;

            // INIT GRID
            int xInd = 0, zInd = 0;
            _grid = new int[_gridSize][];
            for (float z = terrainStart.position.z; z <= terrainEnd.position.z; z++) {
                _grid[zInd] = new int[_gridSize];
                for (float x = terrainStart.position.x; x <= terrainEnd.position.x; x++) {
                    Physics.Raycast(new Vector3(x, castHeight, z), Vector3.down,
                        out var hitInfo, 100f, 1 << 8);
                    _grid[zInd][xInd] = hitInfo.point.y > 0.01f || hitInfo.point.y < -0.01f ? 1 : 0;
                    xInd++;
                }
                xInd = 0;
                zInd++;
            }
            
            terrainStart.position = halfVec;
            terrainEnd.position = Vector3.right * terrainOptions.width + Vector3.forward * terrainOptions.height - halfVec;
            

            _wayPoints = new List<Vector3>();

            _wayPoints = new List<Vector3>();

            var linesCount = _grid.Length;
            var colsCount = _grid[0].Length;
            var costMatrixWidth = linesCount * colsCount;

            _costMatrix = new NativeArray<float>(costMatrixWidth * costMatrixWidth, Allocator.Persistent);
            _heuristicMatrix = new NativeArray<float>(costMatrixWidth, Allocator.Persistent);

            for (var i = 0; i < linesCount; i++) {
                for (var j = 0; j < colsCount; j++) {
                    var sourceIdx = i * colsCount + j;

                    for (var i1 = 0; i1 < linesCount; i1++) {
                        for (var j1 = 0; j1 < colsCount; j1++) {
                            var targetIdx = i1 * colsCount + j1;

                            if (_grid[i][j] != 1 && _grid[i1][j1] != 1 && (i == i1 && Mathf.Abs(j - j1) == 1 ||
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
            if (pos.x < 0) pos.x = 0;
            if (pos.z < 0) pos.z = 0;
            return Mathf.FloorToInt(pos.z) * _gridSize + Mathf.FloorToInt(pos.x);
        }

        public bool UpdateTransform(Transform unitToMove, Vector3 destination, float unitSpeed) {
            Transform targetTransform = unitToMove.transform;
            bool isAtDestination = false;
            var linesCount = _grid.Length;
            var colsCount = _grid[0].Length;
            var costMatrixWidth = linesCount * colsCount;

            //Update Heuristic Matrix to target node
            for (var i = 0; i < linesCount; i++) {
                for (var j = 0; j < colsCount; j++) {
                    var sourceIdx = i * colsCount + j;

                    _heuristicMatrix[sourceIdx] = Mathf.Abs(i - Mathf.FloorToInt(destination.z)) +
                                                 Mathf.Abs(j - Mathf.FloorToInt(destination.x));
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
                var nodePos = new Vector3(node % _gridSize + 0.5f, targetTransform.position.y, Mathf.FloorToInt(node / (float) _gridSize) + 0.5f);
                _wayPoints.Add(nodePos);
            }

            job.exploredNodesCount.Dispose();
            job.bestCost.Dispose();
            job.bestPath.Dispose();

            // Show Debug Path
            #if UNITY_EDITOR
            var lastPoint = targetTransform.position;
            for (var i = 0; i < _wayPoints.Count; i++) {
                var point = _wayPoints[i];
                Debug.DrawLine(lastPoint, point, Color.green, 1f);

                lastPoint = point;
            }
            #endif

            // Move Target
            if (_wayPoints.Count > 0) {
                var point = _wayPoints[0];
                position = targetTransform.position;
                var vectorToTarget = point - position;
                var distanceToTarget = vectorToTarget.magnitude;

                if (distanceToTarget <= Time.deltaTime * unitSpeed) {
                    targetTransform.position = point;
                    _wayPoints.RemoveAt(0);
                    isAtDestination = true;
                }
                else {
                    targetTransform.position += Time.deltaTime * unitSpeed * UnitLibData.speed * vectorToTarget / distanceToTarget;
                    //unitToMove.SetPosition(unitToMove.transform.position);
                    //_targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
                }
            }
            /*targetTransform.rotation =
                Quaternion.RotateTowards(targetTransform.rotation, _targetRotation, rotateSpeed - Time.deltaTime);*/
            return isAtDestination;
        }
    }
}