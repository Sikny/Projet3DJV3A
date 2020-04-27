using System;
using CustomEvents;
using UnityEngine;
using Utility;

namespace Game {
    public class ShortcutManager : MonoBehaviour {
        [Serializable]
        public class KeyMap {
            public KeyCode selectKey = KeyCode.Mouse0;
            public KeyCode cameraRotateKey = KeyCode.Mouse1;
            public KeyCode shopKey = KeyCode.S;
            public KeyCode inventoryKey = KeyCode.I;
            public KeyCode pauseKey = KeyCode.Escape;
        }

        public KeyMap keyMap;

        [Serializable]
        public class GameEvents {
            public CustomEvent pauseEvent;
            public CustomEvent resumeEvent;

            public CustomEvent toggleInventoryEvent;
            public CustomEvent toggleShopEvent;
            public CustomEvent selectionEvent;
            public CustomEvent cameraRotateOnEvent;
            public CustomEvent cameraRotateOffEvent;            
        }

        public GameEvents gameEvents;
        
        void DoUpdate() {
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

            if (Input.GetKeyDown(keyMap.cameraRotateKey)) {
                gameEvents.cameraRotateOnEvent.Raise();
            }

            if (Input.GetKeyUp(keyMap.cameraRotateKey)) {
                gameEvents.cameraRotateOffEvent.Raise();
            }
        }
    }
}