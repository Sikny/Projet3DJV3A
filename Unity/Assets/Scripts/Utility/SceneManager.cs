using System;
using System.Collections.Generic;
using DG.Tweening;
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
                {"loadLvl", 4}, {"freeMode", 5}
            };
        }
        
        public void LoadScene(string sceneName) {
            if (_storedScenesIds[sceneName] == 1)
            {

                DOVirtual.DelayedCall(1f, (() => {                
                            SoundManager soundManager = GameSingleton.Instance.soundManager;
                            //soundManager.StopPlaying("Level theme");
                            soundManager.StopPlayingAllMusics();
                            soundManager.Play("Menu"); }
                    ));

            }
            else if (_storedScenesIds[sceneName] == 2)
            {
                Shop.Instance.ClearShop();

                Player player = GameSingleton.Instance.GetPlayer();
                player.gamemode = Player.Gamemode.LEVEL;
                SoundManager soundManager = GameSingleton.Instance.soundManager;
                
                soundManager.StopPlayingAllMusics();

                //soundManager.StopPlaying("Menu");
                soundManager.Play("Level theme");

            }else if (_storedScenesIds[sceneName] == 5)
            {

                Player player = GameSingleton.Instance.GetPlayer();
                
                string token = player.token;
                
                if (string.IsNullOrEmpty(token) || token.Length < 8)
                {
                    Popups.instance.Popup("Not connected!", Color.red);
                    Debug.Log("Non-connecté");
                }
                else
                {
                    GameSingleton.Instance.tokenManager.CheckToken(token ,"scene.load.freeMode");
                    Shop.Instance.ClearShop();
                    player.gamemode = Player.Gamemode.ARCADE;
                    SoundManager soundManager = GameSingleton.Instance.soundManager;

                    soundManager.StopPlayingAllMusics();
                    soundManager.Play("Level theme");


                }    
                return; // load somewhere else (need token validation)
            }
            
            
            UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);
        }
    }
}