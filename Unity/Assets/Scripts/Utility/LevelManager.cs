using Game;
using UnityEngine;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        public LevelList levelList;

        public Level loadedLevel;

        private void Awake() {
            GameSingleton.Instance.levelManager = this;
            loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.gameVariables.currentLevel));
            loadedLevel.Init();
        }

        public void NextLevel() {
            GameSingleton.Instance.gameVariables.currentLevel = 
                (GameSingleton.Instance.gameVariables.currentLevel + 1) % levelList.LevelCount;
        }
    }
}
