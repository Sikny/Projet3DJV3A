using System;
using System.Collections.Generic;
using Game;
using Items;
using Sounds;
using UI;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Utility {
    [Serializable]
    public class SceneManager {
        private readonly Dictionary<string, int> _storedScenesIds;

        public SceneManager() {
            _storedScenesIds = new Dictionary<string, int> {
                {"Menu", 1}, {"StoryMode", 2}, {"creator", 3},
                {"loadLvl", 4}, {"freeMode", 5}, {"LevelList",6}
            };
        }

        public void LoadScene(string sceneName) {
            UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);

            Player player = GameSingleton.Instance.GetPlayer();
            SoundManager soundManager = GameSingleton.Instance.soundManager;
            switch (sceneName) {
                case "Menu":
                    //soundManager.StopPlaying("Level theme");
                    soundManager.StopPlayingAllMusics();
                    soundManager.Play("Menu");
                    break;
                case "StoryMode":
                    Shop.Instance.ClearShop();

                    player.gamemode = Player.Gamemode.LEVEL;

                    soundManager.StopPlayingAllMusics();

                    //soundManager.StopPlaying("Menu");
                    soundManager.Play("Level theme");
                    break;
                case "freeMode":
                    string token = player.token;

                    if (string.IsNullOrEmpty(token) || token.Length < 8) {
                        Popups.instance.Popup("Not connected!", Color.red);
#if UNITY_EDITOR
                        Debug.Log("Non connecté");
#endif
                    }
                    else {
                        GameSingleton.Instance.tokenManager.CheckToken(token, "scene.load.freeMode");
                        Shop.Instance.ClearShop();
                        player.gamemode = Player.Gamemode.ARCADE;

                        soundManager.StopPlayingAllMusics();
                        soundManager.Play("Level theme");
                    }
                    break;
                case "loadLvl":
                    GameSingleton.Instance.GetPlayer().gamemode = Player.Gamemode.PERSONNALIZED;
                    break;
            }
        }
    }
}