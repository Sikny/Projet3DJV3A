using System;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Utility {
    [Serializable]
    public class SceneManager {
        private readonly Dictionary<string, int> _storedScenesIds;

        public SceneManager() {
            _storedScenesIds = new Dictionary<string, int> {
                {"Menu", 1}, {"StoryMode", 2}, {"creator", 3},
                {"loadLvl", 4}, {"freeMode", 5}
            };
        }
        
        public void LoadScene(string sceneName) {

            if (_storedScenesIds[sceneName] == 2)
            {
                GameSingleton.Instance.GetPlayer().gamemode = Player.Gamemode.LEVEL;
                GameSingleton.Instance.soundManager.Play("Level theme");

            }else if (_storedScenesIds[sceneName] == 5)
            {
                string token = PlayerPrefs.GetString("connection.token");
                if (token == null || token.Length < 8)
                {
                    Debug.Log("Non-connecté");
                }
                else
                {
                    GameSingleton.Instance.tokenManager.CheckToken(token ,"scene.load.freeMode");
                }
                return; // load somewhere else (need token validation)
            }
            
            UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);
        }
    }
}