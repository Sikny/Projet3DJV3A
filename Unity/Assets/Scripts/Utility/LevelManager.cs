﻿using System;
using Game;
using Items;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        public LevelList levelList;

        public Level loadedLevel;

        public GenRandomParam grp;

        private int _levelCountArcade = 0; 
        private void Start()
        {
            if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE && GameSingleton.Instance.GetPlayer().currentScore == 0)
            {
                
                levelList.ClearLevels();
                GameSingleton.Instance.GetPlayer().currentLevelArcade = 0;
                
                _levelCountArcade = 0;
                GenerateLevel();
  
            }
            else if(GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE)
            {
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevelArcade));
                loadedLevel.Init();
            }
            else
            {
                GameSingleton.Instance.GetPlayer().goldStartLevel = GameSingleton.Instance.GetPlayer().gold;
                GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevel));
                loadedLevel.Init();
            }
        }

        public void NextLevel() {
            if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE)
            {
                GameSingleton.Instance.GetPlayer().currentLevelArcade = 
                    (GameSingleton.Instance.GetPlayer().currentLevelArcade + 1) % levelList.LevelCount;
            }
            else
            {
                if (GameSingleton.Instance.GetPlayer().gold < 50)
                    GameSingleton.Instance.GetPlayer().gold = 50;
                Shop.Instance.ClearShop();
                GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
                ShopManager.instance.UpdateGold();
                GameSingleton.Instance.GetPlayer().currentLevel = 
                    (GameSingleton.Instance.GetPlayer().currentLevel + 1) % levelList.LevelCount;
            }
        }

        public void SetLoadedLevel(Level level)
        {
            loadedLevel = level;
        }

        public void GenerateLevel()
        {
            GameSingleton.Instance.GetPlayer().goldStartLevel = GameSingleton.Instance.GetPlayer().gold;
            GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
            //update gold inventoryUI
            _levelCountArcade++;
            loadedLevel = grp.generateNextLevel(Random.Range(Int32.MinValue, Int32.MaxValue),  _levelCountArcade);
            grp.setDefaultGold(loadedLevel);
            GameSingleton.Instance.levelManager = this;
            loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevelArcade));
            loadedLevel.Init();
            
            GameSingleton.Instance.GetPlayer().currentLevelArcade += 1;

            //generate
            //call when pressing free mode button in menu
            //call at end of level in arcade mode 
        }
    }
}
