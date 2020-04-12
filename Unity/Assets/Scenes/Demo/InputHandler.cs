﻿using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Tests {
    public class InputHandler : MonoBehaviour
    {
        public CustomEvents.CustomEvent myCustomEvent;
        private void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                myCustomEvent.Raise();
            }
        }
    }
}
