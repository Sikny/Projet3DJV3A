using System;
using System.Collections.Generic;
using Game;
using Items;
using leveleditor.rule;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        public LevelList levelList;

        public Level loadedLevel;

        public GenRandomParam grp;

        public int seed;

        public void Start()
        {
            // TODO MOVE IN LEVEL SCRIPT
            //GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
            //ShopManager.instance.UpdateGold();
            //UpgradeManager.instance.UpdateGold();
            Player player = GameSingleton.Instance.GetPlayer();
            if (player.gamemode == Player.Gamemode.ARCADE && player.currentScore == 0)
            {
                
                levelList.ClearLevels();
                player.currentLevelArcade = 0;

                GenerateLevel();
  
            }
            else if(player.gamemode == Player.Gamemode.ARCADE)
            {
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevelArcade));
                loadedLevel.Init();
            }
            else if (player.gamemode == Player.Gamemode.PERSONNALIZED)
            {
                String filename = GameSingleton.Instance.filename;
                Rule r = Rule.readLevel(filename.Split('.')[0]);
                
               loadLevel(r);
            }
            else
            {
                player.goldStartLevel = player.gold;
                player.inventoryStartLevel = player.storyModeInventory;
                //GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(player.currentLevel));
                loadedLevel.Init();
            }
        }

        public void NextLevel()
        {
            Player player = GameSingleton.Instance.GetPlayer();
            if (player.gamemode == Player.Gamemode.ARCADE)
            {
                Shop.Instance.ClearShop();

                player.currentLevelArcade = 
                    (player.currentLevelArcade + 1) % levelList.LevelCount;
            }
            else
            {
                if (player.gold < 50)
                    player.gold = 50;
                Shop.Instance.ClearShop();
                GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
                ShopManager.instance.UpdateGold();
                player.currentLevel = 
                    (player.currentLevel + 1) % levelList.LevelCount;
            }
        }

        public void SetLoadedLevel(Level level)
        {
            loadedLevel = level;
        }

        public void loadLevel(Rule rule)
        {
            Shop.Instance.ClearShop();
            GameSingleton.Instance.GetPlayer().gold = rule.maxBudget;
            GameSingleton.Instance.levelManager = this;

            Level levelNew = grp.levelBase;
            loadedLevel = Instantiate(levelNew);
            loadedLevel.rule = rule;
            loadedLevel.Init();
            
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

                seed = Random.Range(Int32.MinValue, Int32.MaxValue);
                GameSingleton.Instance.GetPlayer().currentSeed = seed;
            }
            loadedLevel = grp.generateNextLevel(seed,  GameSingleton.Instance.GetPlayer().currentLevelArcade);
            
            List<EnemySpawn> enemySpawns = loadedLevel.enemySpawns;


            grp.setDefaultGold(loadedLevel);
            GameSingleton.Instance.levelManager = this;
            
            
            loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevelArcade-1));
            loadedLevel.Init();
           /* foreach (var enemy in  enemySpawns)
            {
                Vector2 spawnPosition = enemy.position;

                float yPosition = loadedLevel.terrainBuilder.terrainOptions.modifierHeightMap[spawnPosition];
                Debug.Log("position :" + yPosition);
                Debug.Log(enemy.position);
            }*/
            loadedLevel = grp.respawnEnnemies(loadedLevel);
        }
    }
}
