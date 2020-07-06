using UnityEngine;
using Utility;

namespace CameraMovement {
    /**
     * <summary>Camera control with right click & move</summary>
     */
    public class CameraController : MonoBehaviour
    {
        
        public float speed = 15;
        public float zoomSpeed = 20f;
        public Camera mainCamera;
        public int minZoom = 10;
        public int maxZoom = 40;
        private float _currentYaw;
        
        private bool _isMovingRight;
        private bool _isMovingLeft;
        private bool _isMovingDown;
        private bool _isMovingUp;

        private bool _invertCameraX;
        private bool _invertCameraY;
        

        private void Start()
        {
            _invertCameraX = GameSingleton.Instance.gameSettings.invertCameraX;
            _invertCameraY = GameSingleton.Instance.gameSettings.invertCameraY;

            _currentYaw = maxZoom / 2f;
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
            _currentYaw = Mathf.Clamp(_currentYaw, minZoom, maxZoom);
            //StoreUnit upgradedUnit = (number == 1) ? _unit.upgrades[0] : _unit.upgrades[1];


            if (_isMovingRight)
            {
                transform.Translate(
                    _invertCameraX
                        ? new Vector3(speed * Time.deltaTime, 0, 0)
                        : new Vector3(-speed * Time.deltaTime, 0, 0), Space.World);
            }

            if (_isMovingLeft)
            {
                transform.Translate(
                    _invertCameraX
                        ? new Vector3(-speed * Time.deltaTime, 0, 0)
                        : new Vector3(speed * Time.deltaTime, 0,0), Space.World);
            }

            if (_isMovingDown)
            {
                transform.Translate(
                    _invertCameraY
                        ? new Vector3(0, 0,-speed * Time.deltaTime)
                        : new Vector3(0, 0,speed * Time.deltaTime), Space.World);
            }

            if (_isMovingUp)
            {
                transform.Translate(
                    _invertCameraY
                        ? new Vector3(0, 0,speed * Time.deltaTime)
                        : new Vector3(0, 0,-speed * Time.deltaTime), Space.World);
            }
        
            mainCamera.fieldOfView = _currentYaw;
        }

    }
}