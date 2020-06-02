using UnityEngine;

namespace Terrain {
    public class Cursor : MonoBehaviour {
        public CursorCell[] cursorCells;

        public void SetPosition(float posX, float posZ) {
            var t = transform;
            float yPos = t.position.y;
            t.position = new Vector3(posX, yPos, posZ);
            foreach (CursorCell cursorCell in cursorCells) {
                cursorCell.SetAutoHeight(yPos);
            }
        }
    }
}
