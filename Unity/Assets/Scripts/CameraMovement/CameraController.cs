using UnityEngine;

namespace CameraMovement {
    /**
     * <summary>Camera control with right click & move</summary>
     */
    public class CameraController : MonoBehaviour
    {
        public Transform centerPoint;
        
        public float speed = 100f;
        public float pitch = 2f;
        
        private float _currentYaw;

        private bool _mousePressed;

        public void SetRotating() {
            _mousePressed = true;
        }

        public void UnsetRotating() {
            _mousePressed = false;
        }

        void Update()
        {
            if(_mousePressed)
                _currentYaw += Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        }

        private Vector3 _centerPointPos;
        private void LateUpdate()
        {
            _centerPointPos = centerPoint.position;
            transform.LookAt(_centerPointPos + Vector3.up * pitch);
            transform.RotateAround(_centerPointPos,Vector3.up, _currentYaw);
            _currentYaw = 0;
        }
    }
}