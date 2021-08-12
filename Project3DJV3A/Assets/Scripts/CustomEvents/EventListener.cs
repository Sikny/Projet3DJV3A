using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
    public class EventListener : MonoBehaviour {
        public CustomEvent customEventSent;
        public UnityEvent action;

        private void OnEnable() {
            customEventSent.AddListener(this);
        }

        private void OnDisable() {
            customEventSent.RemoveListener(this);
        }

        public void EventRaised() {
            action.Invoke();
        }
    }
}
