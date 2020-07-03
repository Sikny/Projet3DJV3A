using System;
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

        public int seed;
        public DateTime begintime;
        
        public int _levelCountArcade = 0; 
        private void Start()
        {
            GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
            ShopManager.instance.UpdateGold();
            UpgradeManager.instance.UpdateGold();
            Player player = GameSingleton.Instance.GetPlayer();
            if (player.gamemode == Player.Gamemode.ARCADE && player.currentScore == 0)
            {
                
                levelList.ClearLevels();
                player.currentLevelArcade = 0;
                _levelCountArcade = 0;
                
                GenerateLevel();
  
            }
            else if(player.gamemode == Player.Gamemode.ARCADE)
            {
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevelArcade));
                loadedLevel.Init();
            }
            else
            {
                player.goldStartLevel = player.gold;
                GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(player.currentLevel));
                loadedLevel.Init();
            }
        }

        public void NextLevel() {
            if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE)
            {
                Shop.Instance.ClearShop();

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
            //GameSingleton.Instance.GetPlayer().goldStartLevel = GameSingleton.Instance.GetPlayer().gold;
            //GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
            //update gold inventoryUI
            GameSingleton.Instance.GetPlayer().currentLevelArcade += 1;
            
            if (GameSingleton.Instance.GetPlayer().currentLevelArcade == 1)
            {
                
                GameSingleton.Instance.GetPlayer().beginGame = DateTime.Now;
                int seed = Random.Range(Int32.MinValue, Int32.MaxValue);
                GameSingleton.Instance.GetPlayer().currentSeed = seed;
            }
            loadedLevel = grp.generateNextLevel(seed,  GameSingleton.Instance.GetPlayer().currentLevelArcade);
            grp.setDefaultGold(loadedLevel);
            GameSingleton.Instance.levelManager = this;
            
            
            loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevelArcade-1));
            loadedLevel.Init();
            
            

            //generate
            //call when pressing free mode button in menu
            //call at end of level in arcade mode 
        }
    }
}
