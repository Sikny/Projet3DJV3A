using UnityEngine;

namespace Terrain {
    public class CursorCell : MonoBehaviour
    {

        public float posY;
        
        public void SetAutoHeight() {
            RaycastHit hit;
            Vector3 position = transform.position;
            if (Physics.BoxCast(position+Vector3.up*10f, transform.localScale, Vector3.down, out hit,
                Quaternion.identity, 20f, 1 << 8)) {
                position.y = hit.point.y+0.1f;
                posY = position.y;
                gameObject.SetActive(true);
            } else {
                gameObject.SetActive(false);
            }
            transform.position = position;
        }

        public bool IsOnGround() {
            return Physics.Raycast(transform.position + Vector3.up, Vector3.down, 2f, 1 << 8);
        }
    }
}
