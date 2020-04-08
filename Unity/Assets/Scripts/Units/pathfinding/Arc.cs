namespace AStar
{
    public class Arc
    {
        internal Node From { get; set; }
        internal Node To { get; set; }
        internal double Cost { get; set; }

        public Arc(Node from, Node to, double cost)
        {
            this.From = from;
            this.To = to;
            this.Cost = cost;
        }
    }
}