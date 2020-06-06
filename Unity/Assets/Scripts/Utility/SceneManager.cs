using System;
using System.Collections.Generic;
using Game;
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

            if (_storedScenesIds[sceneName] == 2)
            {
                GameSingleton.Instance.GetPlayer().gamemode = Player.Gamemode.LEVEL;
            }else if (_storedScenesIds[sceneName] == 5)
            {
                GameSingleton.Instance.GetPlayer().gamemode = Player.Gamemode.ARCADE;
            }
            
            UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);
        }
    }
}