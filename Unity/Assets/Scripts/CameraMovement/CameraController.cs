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

        private Vector3 centerPointPos;
        private void LateUpdate()
        {
            centerPointPos = centerPoint.position;
            transform.LookAt(centerPointPos + Vector3.up * pitch);
            transform.RotateAround(centerPointPos,Vector3.up, currentYaw);
            currentYaw = 0;
        }
    }
}