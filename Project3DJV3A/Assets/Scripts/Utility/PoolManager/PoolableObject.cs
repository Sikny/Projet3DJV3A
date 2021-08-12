using UnityEngine;

namespace Utility.PoolManager {
    public class PoolableObject : MonoBehaviour {
        public bool IsActive() {
            return gameObject.activeInHierarchy;
        }

        public virtual void Init() {
            gameObject.SetActive(true);
        }

        public virtual void DeInit() {
            gameObject.SetActive(false);
        }
    }
}