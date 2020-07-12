using UnityEngine;

namespace Units {
    public class Spawner : MonoBehaviour {
        public GameObject spriteRenderer;
        public GameObject spriteMask;

        private Vector3 _firstScale;

        private void Awake() {
            _firstScale = spriteRenderer.transform.localScale;
        }

        public void UpdateScale(float time) { // time between 0 & 1
            float clamped = Mathf.Clamp01(time);
            spriteRenderer.transform.localScale = (clamped + 0.01f) * _firstScale;
            spriteMask.transform.localScale = clamped * _firstScale;
        }
    }
}
