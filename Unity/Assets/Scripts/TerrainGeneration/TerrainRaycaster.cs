using UnityEngine;

namespace TerrainGeneration {
    public class TerrainRaycaster : MonoBehaviour {
        private Camera _cam;

        private void Awake() {
            _cam = Camera.main;
        }
        
        
        private void OnMouseOver() {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100)) {
                Vector3 hitPos = hit.point;
                float xHit = Mathf.Floor(hitPos.x);
                float zHit = Mathf.Floor(hitPos.z);
                int posX = (int)xHit + 15 ;
                int posZ = (int)zHit + 15 ;
                TerrainGrid.Instance.SelectZone(posZ, posX);
            }
        }
    }
}