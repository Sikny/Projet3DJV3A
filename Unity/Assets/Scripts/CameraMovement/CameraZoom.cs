using UnityEngine;

namespace CameraMovement {
    /**
     * <summary>Camera zoom with mouse wheel</summary>
     */
    public class CameraZoom : MonoBehaviour {
        public Transform centerPoint;
        public Vector3 offset;
        private float currentZoom = 10f;
        public float zoomSpeed = 4f;
        public float minZoom = 4f;
        public float maxZoom = 50f;
        
        private void Update() {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            transform.position = centerPoint.position - offset * currentZoom;
        }
    }
}
