using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;

namespace AStar
{
    public class Algorithm
    {
        
        readonly Stopwatch sw = new Stopwatch();
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
            sw.Reset();
            
            graph.ComputeEstimatedDistance();
            
            List<Node> nodeToVisit = graph.NodesList();
            bool exitReached = false;
            while (nodeToVisit.Count != 0 && !exitReached)
            {
                sw.Start();
                Node currentNode = nodeToVisit.FirstOrDefault();
                for (var index = 0; index < nodeToVisit.Count; index++)
                {
                    var newNode = nodeToVisit[index];
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
                    //List<Arc> arcsFromCurrentNode = graph.ArcsList(currentNode);
                    Arc[] arcsFromCurrentNode = graph.ArcsList(currentNode);
                    for(var i = 0; i < arcsFromCurrentNode.Length; i++)
                    {
                        var arc = arcsFromCurrentNode[i];
                        if(arc == null ) break;
                        if (arc.From.DistanceTraveled + arc.Cost < arc.To.DistanceTraveled)
                        {
                            arc.To.DistanceTraveled = arc.From.DistanceTraveled + arc.Cost;
                            arc.To.Previous = arc.From;
                        }
                    }

                    nodeToVisit.Remove(currentNode);
                    
                }
                sw.Stop();
            }
            
            //Debug.Log(sw.ElapsedMilliseconds);
        }
    }
}