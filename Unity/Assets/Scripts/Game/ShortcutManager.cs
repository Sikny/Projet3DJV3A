using CustomEvents;
using UnityEngine;
using Utility;

namespace Game {
    public class ShortcutManager : MonoBehaviour {
        public CustomEvent pauseEvent;
        public CustomEvent resumeEvent;

        public CustomEvent toggleInventoryEvent;
        public CustomEvent toggleShopEvent;

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                toggleShopEvent.Raise();
            }

            if (Input.GetKeyDown(KeyCode.I)) {
                toggleInventoryEvent.Raise();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (GameSingleton.Instance.gamePaused) {
                    resumeEvent.Raise();
                }
                else {
                    pauseEvent.Raise();
                }
            }
        }
    }
}