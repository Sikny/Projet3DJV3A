using UnityEngine;
using Utility.PoolManager;

namespace Units {
    public class Spawner : PoolableObject {
        public Transform spriteRenderer;
        public Transform spriteMask;

        private Vector3 _firstScale;
        private Vector3 _offset = Vector3.one * 0.015f;

        public static float timeToSpawn = 2.5f;
        private float _time;

        private void Awake() {
            _firstScale = spriteRenderer.localScale;
            _time = timeToSpawn;
        }

        public void UpdateTime() {
            _time -= Time.deltaTime;
            float lerp = Mathf.InverseLerp(0f, timeToSpawn, _time);
            spriteRenderer.localScale = lerp * _firstScale;
            spriteMask.localScale = lerp * _firstScale - _offset;
            if(_time > timeToSpawn)
                DeInit();
        }
    }
}
