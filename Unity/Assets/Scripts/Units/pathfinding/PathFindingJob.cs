using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Units.PathFinding {
    [BurstCompile]
    public struct PathFindingJob : IJob
    {
        [ReadOnly] public NativeArray<float> costMatrix;
        [ReadOnly] public NativeArray<float> heuristicMatrix;
        [WriteOnly] public NativeArray<float> bestCost;
        [WriteOnly] public NativeArray<int> exploredNodesCount;
        [WriteOnly] public NativeList<int> bestPath;

        public int nodesCount;
        public int startNodeId;
        public int endNodeId;

        public void Execute()
        {
            var nodesLeftToExploreIds = new NativeList<int>(nodesCount, Allocator.Temp);
            var nodesLeftToExploreCosts = new NativeList<float>(nodesCount, Allocator.Temp);
            var nodesPreviousElementsIds = new NativeArray<int>(nodesCount, Allocator.Temp);
            var exploredNodesCountTmp = 0;

            for (var i = 0; i < nodesCount; i++)
            {
                nodesLeftToExploreIds.Add(i);
                nodesLeftToExploreCosts.Add(float.MaxValue);
            }

            nodesLeftToExploreCosts[startNodeId] = 0;

            while (true)
            {
                var chosenNodeIdx = -1;
                var chosenNodeCost = float.MaxValue;
                var chosenNodeCostWithHeuristic = float.MaxValue;

                for (var i = 0; i < nodesLeftToExploreCosts.Length; i++)
                {
                    var candidateNodeCost = nodesLeftToExploreCosts[i];
                    var candidateNodeCostWithHeuristic = candidateNodeCost + heuristicMatrix[nodesLeftToExploreIds[i]];
                    if (candidateNodeCostWithHeuristic < chosenNodeCostWithHeuristic
                        || Math.Abs(candidateNodeCostWithHeuristic - chosenNodeCostWithHeuristic) < 0.0001f &&
                        candidateNodeCost > chosenNodeCost)
                    {
                        chosenNodeIdx = i;
                        chosenNodeCost = candidateNodeCost;
                        chosenNodeCostWithHeuristic = candidateNodeCostWithHeuristic;
                    }
                }

                if (chosenNodeIdx == -1)
                {
                    exploredNodesCount[0] = exploredNodesCountTmp;
                    bestCost[0] = chosenNodeCost;
                    return;
                }

                var chosenNodeId = nodesLeftToExploreIds[chosenNodeIdx];

                if (chosenNodeId == endNodeId)
                {
                    exploredNodesCount[0] = exploredNodesCountTmp;
                    bestCost[0] = chosenNodeCost;

                    var nodeId = endNodeId;
                    var inversedPathQueue = new NativeList<int>(Allocator.Temp);
                    inversedPathQueue.Add(nodeId);
                    while (nodeId != startNodeId)
                    {
                        nodeId = nodesPreviousElementsIds[nodeId];
                        inversedPathQueue.Add(nodeId);
                    }

                    var pathLength = inversedPathQueue.Length;
                    for (var n = 0; n < pathLength; n++)
                    {
                        bestPath.Add(inversedPathQueue[pathLength - 1 - n]);
                    }

                    return;
                }

                for (var j = 0; j < nodesLeftToExploreCosts.Length; j++)
                {
                    var targetId = nodesLeftToExploreIds[j];
                    var transitionCost = costMatrix[chosenNodeId * nodesCount + targetId];
                    if (transitionCost >= float.MaxValue)
                    {
                        continue;
                    }

                    var candidateCost = transitionCost + chosenNodeCost;
                    if (candidateCost < nodesLeftToExploreCosts[j])
                    {
                        nodesLeftToExploreCosts[j] = candidateCost;
                        nodesPreviousElementsIds[nodesLeftToExploreIds[j]] = chosenNodeId;
                    }
                }
            
                nodesLeftToExploreIds.RemoveAtSwapBack(chosenNodeIdx);
                nodesLeftToExploreCosts.RemoveAtSwapBack(chosenNodeIdx);
                exploredNodesCountTmp++;
            }
        }
    }
}
