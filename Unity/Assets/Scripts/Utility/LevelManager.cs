using Game;
using UnityEngine;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private LevelList levelList;
        public int currentLevel;

        private Level _loadedLevel;

        private void Awake() {
            _loadedLevel = Instantiate(levelList.GetLevel(currentLevel));
        }
    }
}
