using System;
using System.Collections.Generic;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Utility {
    [Serializable]
    public class SceneManager {
        private readonly Dictionary<string, int> _storedScenesIds;

        public SceneManager() {
            _storedScenesIds = new Dictionary<string, int> {
                {"Menu", 1}, {"StoryMode", 2}, {"creator", 3},
                {"loadLvl", 4}, {"personnalizedMap", 5}
            };
        }
        
        public void LoadScene(string sceneName) {
            if(sceneName == "StoryMode")
                GameSingleton.Instance.soundManager.Play("Level theme");
            UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);
        }
    }
}