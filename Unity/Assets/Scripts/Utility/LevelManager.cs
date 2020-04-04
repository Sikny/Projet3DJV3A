using Game;
using UnityEngine;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private LevelList levelList;

        private Level _loadedLevel;

        private void Awake() {
            GameSingleton.Instance.levelManager = this;
            _loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.gameVariables.currentLevel));
        }

        public void NextLevel() {
            GameSingleton.Instance.gameVariables.currentLevel = 
                (GameSingleton.Instance.gameVariables.currentLevel + 1) % levelList.LevelCount;
        }
    }
}
