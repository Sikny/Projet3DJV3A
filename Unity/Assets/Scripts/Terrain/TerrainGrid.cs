using UnityEngine;

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

        public Transform cursor;

        public void SelectZone(int posZ, int posX) {
            cursor.position = new Vector3(posX-Width/2, cursor.position.y, posZ-Height/2);
        }
    }
}
