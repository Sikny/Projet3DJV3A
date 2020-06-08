namespace Terrain {
    public class TerrainGrid {
        private static TerrainGrid _instance;
        
        public static TerrainGrid Instance {
            get {
                if(_instance == null)
                    _instance = new TerrainGrid();
                return _instance;
            }
        }

        public static int Width { get; set; }

        public static int Height { get; set; }

        public Cursor cursor;

        public void SelectZone(int posZ, int posX) {
            cursor.SetPosition(posX-Width/2+0.5f,posZ-Height/2+0.5f);
        }
    }
}
