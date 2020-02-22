using UnityEngine;

namespace TerrainGeneration {
    public class Grid
    {
        private static Grid _instance;
        private Grid()
        {
            GridArray = new int[500,500];
            PrintGrid = "\n";

            TileX = 5;
            TileZ = 5;
        
            CubeRenderers = new Renderer[500,500];
        }
        
        public string PrintGrid { get; set; }
        public int[,] GridArray { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileX { get; set; }
        public int TileZ { get; set; }
        public Renderer[,] CubeRenderers { get; set; }
        public static Grid GetInstance()
        {
            if(_instance == null)
                _instance = new Grid();
        
            return _instance;
        }
    
    
    }
}
