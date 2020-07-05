using UnityEngine;

namespace Terrain {
    public class Cursor : MonoBehaviour {
        public CursorCell[] cursorCells;

        public void SetPosition(float posX, float posZ) {
            var t = transform;
            t.position = new Vector3(posX, 0, posZ);
            foreach (CursorCell cursorCell in cursorCells) {
                cursorCell.SetAutoHeight();
            }
        }
    }
}
