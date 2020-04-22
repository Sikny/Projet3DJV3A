using Game;
using UnityEngine;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        public LevelList levelList;

        public Level loadedLevel;

        private void Awake() {
            GameSingleton.Instance.levelManager = this;
            loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevel));
            loadedLevel.Init();
        }

        public void NextLevel() {
            GameSingleton.Instance.GetPlayer().currentLevel = 
                (GameSingleton.Instance.GetPlayer().currentLevel + 1) % levelList.LevelCount;
        }
    }
}
