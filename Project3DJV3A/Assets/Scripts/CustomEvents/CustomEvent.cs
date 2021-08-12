using System.Collections.Generic;
using UnityEngine;

namespace CustomEvents {
    [CreateAssetMenu(fileName = "CustomEvent", menuName = "ScriptableObject/NewEvent")]
    public class CustomEvent : ScriptableObject {
        private readonly List<EventListener> _listeners = new List<EventListener>();

        public void AddListener(EventListener listener) {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void RemoveListener(EventListener listener) {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }

        public void Raise() {
            int listenerCount = _listeners.Count;
            for (int i = 0; i < listenerCount; i++) {
                _listeners[i].EventRaised();
            }
        }
    }
}
