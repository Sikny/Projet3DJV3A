using System;

namespace AStar
{
    public class Node
    {
        // Start is called before the first frame update
        private Node previous = null;

        internal Node Previous
        {
            get => previous;
            set => previous = value;
        }
        private double distanceTraveled = Double.PositiveInfinity;

        public double DistanceTraveled
        {
            get => distanceTraveled;
            set => distanceTraveled = value;
        }

        public double EstimatedDistance { get; set; }
    }
}