using System;
using Units;
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

        public Cursor cursor;

        public void SelectZone(int posZ, int posX) {
            cursor.SetPosition(posX+0.5f,posZ+0.5f);
        }

        public Vector3 GetClosestValidPosition(Vector3 position) {
            int posX = Mathf.FloorToInt(position.x), posZ = Mathf.FloorToInt(position.z);
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    Vector3 currentPos = Vector3.right * (x + posX) + Vector3.forward * (y + posZ) + Vector3.up * 50f;
                    Physics.Raycast(currentPos, Vector3.down, out var hitInfo, 100f, 1 << 8);
                    if (Math.Abs(hitInfo.point.y) > 0.01f) {
                        return new Vector3(-1, -1, -1);
                    }
                }
            }
            return new Vector3(posX, SystemUnit.YPos, posZ);
        }
    }
}
