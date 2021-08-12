using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Items;
using Language;
using Sounds;
using UI;
using Units;
using Units.Controllers.Soldier;
using UnityEngine;
using Random = System.Random;
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
            Player player = GameSingleton.Instance.GetPlayer();
            SoundManager soundManager = GameSingleton.Instance.soundManager;
            
            PoolManager.PoolManager.Instance().ReleaseAll();

            switch (sceneName) {
                case "Menu":
                    //soundManager.StopPlaying("Level theme");
                    soundManager.StopPlayingAllMusics();
                    soundManager.Play("Menu");

                    if(player.gamemode != Player.Gamemode.LEVEL)
                    {
                        player.arcadeModeInventory.Clear();
                        Shop.Instance.ClearShop();
                        player.arcadeGold = 150;
                        player.currentLevelArcade = 0;
                    }
                    else
                    {
                        player.storyModeInventory = player.inventoryBackup;
                        player.gold = player.goldStartLevel;
                        //player.storyModeInventory.Clear();
                        
                    }

                    player.gamemode = Player.Gamemode.NONE;
                    UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);
                    break;
                case "StoryMode":
                    if (player.gamemode == Player.Gamemode.LEVEL) {    // button restart
                        player.storyModeInventory = player.inventoryBackup;
                        player.gold = player.goldStartLevel;
                    }
                    Shop.Instance.ClearShop();
                   
                    player.gamemode = Player.Gamemode.LEVEL;

                    soundManager.StopPlayingAllMusics();

                    //soundManager.StopPlaying("Menu");
                    soundManager.Play("Level theme");
                    UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);
                    break;
                case "freeMode":
                    string token = player.token;

                    if (string.IsNullOrEmpty(token) || token.Length < 8) {
                        Popups.instance.Popup(Traducer.Translate("Not connected!"), Color.red);
                    }
                    else {
                        GameSingleton.Instance.tokenManager.CheckToken(token, "scene.load.freeMode");
                        Shop.Instance.ClearShop();
                        
                        player.currentLevelArcade = 0;
                        player.arcadeModeInventory.Clear();
                        player.arcadeGold = 150;

                        player.gamemode = Player.Gamemode.ARCADE;

                        soundManager.StopPlayingAllMusics();
                        soundManager.Play("Level theme");
                        UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);
                    }
                    break;
                case "loadLvl":
                    GameSingleton.Instance.GetPlayer().gamemode = Player.Gamemode.PERSONNALIZED;
                    player.arcadeModeInventory.Clear();
                    player.arcadeGold = 150;
                    UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);
                    break;
                case "creator":
                    UnitySceneManager.LoadScene(_storedScenesIds[sceneName]);
                    break;
            }
        }
    }
}