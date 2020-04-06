using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "New Unit", menuName = "ScriptableObjects/Unit")]
    public class StoreUnit : Item {
        public GameObject inGamePrefab;

        public void Spawn(Vector3 position) {
            Instantiate(inGamePrefab, position, Quaternion.identity);
        }
    }
}