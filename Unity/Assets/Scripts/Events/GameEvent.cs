using UnityEngine;
using UnityEngine.Events;

namespace Events {
    [CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent")]
    public class GameEvent : ScriptableObject {
        private readonly UnityEvent _event = new UnityEvent();

        public void Invoke() {
            _event.Invoke();
        }
    }
}
