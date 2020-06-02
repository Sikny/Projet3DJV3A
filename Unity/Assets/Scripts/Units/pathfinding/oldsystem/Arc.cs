namespace AStar
{
    public class Arc
    {
        public Node From;
        public Node To;
        public double Cost;

        public Arc(Node from, Node to, double cost)
        {
            this.From = from;
            this.To = to;
            this.Cost = cost;
        }
    }
}