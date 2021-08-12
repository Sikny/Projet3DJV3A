using UnityEngine;

namespace CustomEvents {
    public class CustomEventTrigger : MonoBehaviour {
        public CustomEvent targetEvent;
        public void DoTrigger() {
            targetEvent.Raise();
        }
    }
}
