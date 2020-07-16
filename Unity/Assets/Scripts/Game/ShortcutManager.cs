using System;
using CustomEvents;
using UnityEngine;
using Utility;

namespace Game {
    public class ShortcutManager : MonoBehaviour {
        [Serializable]
        public class KeyMap {
            public KeyCode selectKey = KeyCode.Mouse0;
            public KeyCode shopKey = KeyCode.U;
            public KeyCode inventoryKey = KeyCode.I;
            public KeyCode pauseKey = KeyCode.Escape;
            
            public KeyCode arrowCameraRight = KeyCode.RightArrow;
            public KeyCode arrowCameraLeft = KeyCode.LeftArrow;
            public KeyCode arrowCameraDown = KeyCode.DownArrow;
            public KeyCode arrowCameraUp = KeyCode.UpArrow;
            public KeyCode cameraRight = KeyCode.D;
            public KeyCode cameraLeft = KeyCode.Q;
            public KeyCode cameraDown = KeyCode.S;
            public KeyCode cameraUp = KeyCode.Z;
        }

        public KeyMap keyMap;

        [Serializable]
        public class GameEvents {
            public CustomEvent pauseEvent;
            public CustomEvent resumeEvent;

            public CustomEvent toggleInventoryEvent;
            public CustomEvent toggleShopEvent;
            public CustomEvent selectionEvent;
            public CustomEvent cameraMoveRightOnEvent;
            public CustomEvent cameraMoveRightOffEvent;
            public CustomEvent cameraMoveLeftOnEvent;
            public CustomEvent cameraMoveLeftOffEvent;
            public CustomEvent cameraMoveDownOnEvent;
            public CustomEvent cameraMoveDownOffEvent;
            public CustomEvent cameraMoveUpOnEvent;
            public CustomEvent cameraMoveUpOffEvent;
        }

        public GameEvents gameEvents;
        
        public void DoUpdate() {
            if (Input.GetKeyDown(keyMap.shopKey)) {
                gameEvents.toggleShopEvent.Raise();
            }

            if (Input.GetKeyDown(keyMap.inventoryKey)) {
                gameEvents.toggleInventoryEvent.Raise();
            }

            if (Input.GetKeyDown(keyMap.pauseKey)) {
                if (GameSingleton.Instance.gamePaused) {
                    gameEvents.resumeEvent.Raise();
                }
                else {
                    gameEvents.pauseEvent.Raise();
                }
            }

            if (Input.GetKeyDown(keyMap.selectKey)) {
                gameEvents.selectionEvent.Raise();
            }

            if (Input.GetKeyDown(keyMap.cameraRight) || Input.GetKeyDown(keyMap.arrowCameraRight))
            {
                gameEvents.cameraMoveRightOnEvent.Raise();
            }
            if (Input.GetKeyUp(keyMap.cameraRight) || Input.GetKeyUp(keyMap.arrowCameraRight))
            {
                gameEvents.cameraMoveRightOffEvent.Raise();
            }
            
            if (Input.GetKeyDown(keyMap.cameraLeft) || Input.GetKeyDown(keyMap.arrowCameraLeft))
            {
                gameEvents.cameraMoveLeftOnEvent.Raise();
            }
            if (Input.GetKeyUp(keyMap.cameraLeft) || Input.GetKeyUp(keyMap.arrowCameraLeft))
            {
                gameEvents.cameraMoveLeftOffEvent.Raise();
            }
            
            if (Input.GetKeyDown(keyMap.cameraDown) || Input.GetKeyDown(keyMap.arrowCameraDown))
            {
                gameEvents.cameraMoveDownOnEvent.Raise();
            }
            if (Input.GetKeyUp(keyMap.cameraDown) || Input.GetKeyUp(keyMap.arrowCameraDown))
            {
                gameEvents.cameraMoveDownOffEvent.Raise();
            }
            
            if (Input.GetKeyDown(keyMap.cameraUp) || Input.GetKeyDown(keyMap.arrowCameraUp))
            {
                gameEvents.cameraMoveUpOnEvent.Raise();
            }
            if (Input.GetKeyUp(keyMap.cameraUp) || Input.GetKeyUp(keyMap.arrowCameraUp))
            {
                gameEvents.cameraMoveUpOffEvent.Raise();
            }


                

        }
    }
}