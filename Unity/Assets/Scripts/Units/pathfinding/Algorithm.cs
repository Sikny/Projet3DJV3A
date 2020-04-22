using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar
{
    public class Algorithm
    {
        protected Graph graph;
        
        public Algorithm(Graph graph)
        {
            this.graph = graph;
        }

        public void Solve()
        {
            graph.Clear();
            
            
            Run();
            //Debug.Log(graph.ExitNode.DistanceTraveled);
        }

        private void Run()
        {
            graph.ComputeEstimatedDistance();
            List<Node> nodeToVisit = graph.NodesList();
            bool exitReached = false;
            while (nodeToVisit.Count != 0 && !exitReached)
            {
                Node currentNode = nodeToVisit.FirstOrDefault();
                foreach (var newNode in nodeToVisit)
                {
                    if ((newNode.DistanceTraveled + newNode.EstimatedDistance) <
                        (currentNode.DistanceTraveled + currentNode.EstimatedDistance))
                    {
                        currentNode = newNode;
                    }
                    
                }

                if (currentNode == graph.ExitNode)
                {
                    exitReached = true;
                }
                else
                {
                    List<Arc> arcsFromCurrentNode = graph.ArcsList(currentNode);
                    foreach (var arc in arcsFromCurrentNode)
                    {
                        if (arc.From.DistanceTraveled + arc.Cost < arc.To.DistanceTraveled)
                        {
                            arc.To.DistanceTraveled = arc.From.DistanceTraveled + arc.Cost;
                            arc.To.Previous = arc.From;
                        }
                    }

                    nodeToVisit.Remove(currentNode);
                }
            }
        }
    }
}