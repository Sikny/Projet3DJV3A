using Game;
using UnityEngine;

namespace CameraMovement {
    public class CameraController : MonoBehaviour
    {
        public Transform centerPoint;
        
        public float speed = 100f;
        public float pitch = 2f;
        
        private float currentYaw = 0f;

        private bool _mousePressed;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1)) //right click
            {
                _mousePressed = true;
            }
            if (Input.GetMouseButtonUp(1)) {
                _mousePressed = false;
            }
            if(_mousePressed)
                currentYaw += Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        }

        private void LateUpdate()
        {
            transform.LookAt(centerPoint.position + Vector3.up * pitch);
            transform.RotateAround(centerPoint.position,Vector3.up, currentYaw);
            currentYaw = 0;
        }
    }
}