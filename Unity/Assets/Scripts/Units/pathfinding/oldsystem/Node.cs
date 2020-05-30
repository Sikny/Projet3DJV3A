using System;

namespace AStar
{
    public class Node
    {
        // Start is called before the first frame update
        public Node Previous = null;

        
        public double DistanceTraveled = Double.PositiveInfinity;

        

        public double EstimatedDistance { get; set; }
    }
}