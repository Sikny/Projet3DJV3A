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
        
        private const int CursorSize = 3;
        
        public void AddTerrainUnit(TerrainUnit tU, int posZ, int posX) {
            terrainUnits[posZ, posX] = tU;
        }
        
        public void SelectZone(int posZ, int posX) {
            if (TileZ > 0 && TileZ > 0) {
                for (int i = -CursorSize / 2; i <= CursorSize / 2; i++) {
                    if (TileX - Mathf.Abs(i) < 0 && i < 0 || TileX + Mathf.Abs(i) >= Width && i > 0)
                        continue;
                    for (int j = -CursorSize / 2; j <= CursorSize / 2; j++) {
                        if (TileZ - Mathf.Abs(j) < 0 && j < 0 || TileZ + Mathf.Abs(j) >= Height && j > 0)
                            continue;
                        terrainUnits[TileZ + j, TileX + i].DeselectUnit();
                    }
                }
            }
            TileZ = posZ;
            TileX = posX;
            for (int i = -CursorSize/2; i <= CursorSize/2; i++) {
                if(TileX-Mathf.Abs(i) < 0 && i < 0 || TileX+Mathf.Abs(i) >= Width && i > 0)
                    continue;
                for (int j = -CursorSize/2; j <= CursorSize/2; j++) {
                    if (TileZ - Mathf.Abs(j) < 0 && j < 0 || TileZ + Mathf.Abs(j) >= Height && j > 0)
                        continue;
                    terrainUnits[TileZ+j, TileX+i].SelectUnit();
                }
            }
        }
    }
}
