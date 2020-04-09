
using System;

namespace AStar
{

    public enum TileType
    {
        Grass
    };
    
    public class Tile : Node
    {
        protected TileType tileType;
        public int Row { get; set; }
        public int Col { get; set; }

        public Tile(TileType type,int row, int col)
        {
            tileType = type;
            Row = row;
            Col = col;
        }

        public bool IsValidPath()
        {
            return tileType.Equals(TileType.Grass);
        }

        public double Cost()
        {
            switch (tileType)
            {
                case TileType.Grass:
                    return 1;
                default:
                    return Double.PositiveInfinity;
            }
        }
    }
}

