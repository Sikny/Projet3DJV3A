using UnityEngine;

namespace CameraMovement {
    /**
     * <summary>Camera control with right click & move</summary>
     */
    public class CameraController : MonoBehaviour
    {
        public Transform centerPoint;
        
        public float speed = 15;
        public float pitch = 2f;
        public float zoomSpeed = 6f;
        public Camera camera;
        private float _currentYaw;
        private int _minZoom = 10;
        private int _maxZoom = 40;
        private bool _mousePressed;
        
        private bool _isMovingRight;
        private bool _isMovingLeft;
        private bool _isMovingDown;
        private bool _isMovingUp;

        public void SetRotating() {
            _mousePressed = true;
        }

        public void UnsetRotating() {
            _mousePressed = false;
        }

        public void SetMoveRight()
        {
            _isMovingRight = true; 
        }
        public void UnsetMoveRight()
        {
            _isMovingRight = false; 
        }
        
        public void SetMoveLeft()
        {
            _isMovingLeft = true; 
        }
        public void UnsetMoveLeft()
        {
            _isMovingLeft = false; 
        }
        
        public void SetMoveDown()
        {
            _isMovingDown = true; 
        }
        public void UnsetMoveDown()
        {
            _isMovingDown = false; 
        }
        
        public void SetMoveUp()
        {
            _isMovingUp = true; 
        }
        public void UnsetMoveUp()
        {
            _isMovingUp = false; 
        }
        void Update()
        {
 
            _currentYaw -= Input.mouseScrollDelta.y * (zoomSpeed * speed * Time.deltaTime);// Input.GetAxis("Mouse ScrollWheel") * speed * Time.deltaTime;
            _currentYaw = Mathf.Clamp(_currentYaw, _minZoom, _maxZoom);

            if(_isMovingRight)
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0,0), Space.World);
            if(_isMovingLeft)
                transform.Translate(new Vector3(speed * Time.deltaTime, 0,0), Space.World);
            if(_isMovingDown)
                transform.Translate(new Vector3(0, 0,speed * Time.deltaTime), Space.World);
            if(_isMovingUp)
                transform.Translate(new Vector3(0, 0,-speed * Time.deltaTime), Space.World);
        
            camera.fieldOfView = _currentYaw;
        }

    }
}