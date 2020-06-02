
using System;
using UnityEngine;

namespace AStar
{

    public enum TileType
    {
        Grass,
        Wall,
        Pente
    };
    
    public class Tile : Node
    {
        protected TileType tileType;
        public int Row { get; set; }
        public int Col { get; set; }

        public Vector3 Pos { get; set; }
        
        public Tile(TileType type,int row, int col, Vector3 pos)
        {
            tileType = type;
            Row = row;
            Col = col;
            Pos = pos;
        }

        public bool IsValidPath()
        {
            return tileType.Equals(TileType.Grass) || tileType.Equals(TileType.Pente);
        }

        public double Cost()
        {
            switch (tileType)
            {
                case TileType.Grass:
                    return 1;
                case TileType.Pente:
                    return Pos.y+1;
                default:
                    return Double.PositiveInfinity;
            }
        }
    }
}

