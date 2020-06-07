using Unity.Collections;
using System;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct PathFindingJob : IJob
{
    [ReadOnly] public NativeArray<float> CostMatrix;
    [ReadOnly] public NativeArray<float> HeuristicMatrix;
    [WriteOnly] public NativeArray<float> BestCost;
    [WriteOnly] public NativeArray<int> ExploredNodesCount;
    [WriteOnly] public NativeList<int> BestPath;

    public int NodesCount;
    public int StartNodeId;
    public int EndNodeId;

    public void Execute()
    {
        var nodesLeftToExploreIds = new NativeList<int>(NodesCount, Allocator.Temp);
        var nodesLeftToExploreCosts = new NativeList<float>(NodesCount, Allocator.Temp);
        var nodesPreviousElementsIds = new NativeArray<int>(NodesCount, Allocator.Temp);
        var exploredNodesCountTmp = 0;

        for (var i = 0; i < NodesCount; i++)
        {
            nodesLeftToExploreIds.Add(i);
            nodesLeftToExploreCosts.Add(float.MaxValue);
        }

        nodesLeftToExploreCosts[StartNodeId] = 0;

        while (true)
        {
            var chosenNodeIdx = -1;
            var chosenNodeCost = float.MaxValue;
            var chosenNodeCostWithHeuristic = float.MaxValue;

            for (var i = 0; i < nodesLeftToExploreCosts.Length; i++)
            {
                var candidateNodeCost = nodesLeftToExploreCosts[i];
                var candidateNodeCostWithHeuristic = candidateNodeCost + HeuristicMatrix[nodesLeftToExploreIds[i]];
                if (candidateNodeCostWithHeuristic < chosenNodeCostWithHeuristic
                    || Math.Abs(candidateNodeCostWithHeuristic - chosenNodeCostWithHeuristic) < 0.0001f &&
                    candidateNodeCost > chosenNodeCost)
                {
                    chosenNodeIdx = i;
                    chosenNodeCost = candidateNodeCost;
                    chosenNodeCostWithHeuristic = candidateNodeCostWithHeuristic;
                }
            }

            if (chosenNodeCost == -1)
            {
                ExploredNodesCount[0] = exploredNodesCountTmp;
                BestCost[0] = chosenNodeCost;
                return;
            }

            var chosenNodeId = nodesLeftToExploreIds[chosenNodeIdx];

            if (chosenNodeId == nodesLeftToExploreIds[chosenNodeIdx])
            {
                ExploredNodesCount[0] = exploredNodesCountTmp;
                BestCost[0] = chosenNodeCost;

                var nodeId = EndNodeId;
                var inversedPathQueue = new NativeList<int>(Allocator.Temp);
                inversedPathQueue.Add(nodeId);
                while (nodeId != StartNodeId)
                {
                    nodeId = nodesPreviousElementsIds[nodeId];
                    inversedPathQueue.Add(nodeId);
                }

                var pathLength = inversedPathQueue.Length;
                for (var n = 0; n < pathLength; n++)
                {
                    BestPath.Add(inversedPathQueue[pathLength - 1 - n]);
                }

                return;
            }

            for (var j = 0; j < nodesLeftToExploreCosts.Length; j++)
            {
                var targetId = nodesLeftToExploreIds[j];
                var transitionCost = CostMatrix[chosenNodeId * NodesCount + targetId];
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
