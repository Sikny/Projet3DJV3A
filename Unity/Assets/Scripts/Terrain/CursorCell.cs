using UnityEngine;

namespace Terrain {
    public class CursorCell : MonoBehaviour
    {

        public float posY;
        
        public void SetAutoHeight(float defaultY) {
            RaycastHit hit;
            Vector3 position = transform.position;
            if (Physics.Raycast(position+Vector3.up*10, Vector3.down, out hit, 20f, 1 << 8)) {
                position.y = hit.point.y+0.1f;
                posY = position.y;
            } else {
                position.y = defaultY+0.1f;
            }
            transform.position = position;
        }
    }
}
