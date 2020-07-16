using UnityEngine;

namespace Utility.PoolManager {
    public class PoolableObject : MonoBehaviour {
        public bool IsActive() {
            return gameObject.activeInHierarchy;
        }

        public void Init() {
            gameObject.SetActive(true);
        }

        public void DeInit() {
            gameObject.SetActive(false);
        }
    }
}