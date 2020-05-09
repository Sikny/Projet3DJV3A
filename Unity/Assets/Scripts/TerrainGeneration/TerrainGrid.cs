using UnityEngine;

namespace TerrainGeneration {
    public class TerrainGrid {
        private static TerrainGrid _instance;
        
        public static TerrainGrid Instance {
            get {
                if(_instance == null)
                    _instance = new TerrainGrid();
                return _instance;
            }
        }
        
        private TerrainGrid() {
            GridArray = new int[Height,Width];
            terrainUnits = new TerrainUnit[Height,Width];
            TileX = -1;
            TileZ = -1;
        }
        
        public int[,] GridArray { get; set; }
        
        public static int Width { get; set; }

        public static int Height { get; set; }
        
        public int TileX { get; set; }
        
        public int TileZ { get; set; }
        
        private readonly TerrainUnit[,] terrainUnits;

        public Transform cursor;
        
        public void AddTerrainUnit(TerrainUnit tU, int posZ, int posX) {
            terrainUnits[posZ, posX] = tU;
        }
        
        public void SelectZone(int posZ, int posX) {
            TileZ = posZ;
            TileX = posX;
            cursor.position = new Vector3(posX-Width/2, cursor.position.y, posZ-Height/2);
        }
    }
}
