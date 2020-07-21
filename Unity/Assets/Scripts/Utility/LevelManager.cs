using System;
using System.Collections.Generic;
using Game;
using Game.arcade;
using Items;
using LevelEditor.Rule;
using Terrain;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        public LevelList levelList;

        public Level loadedLevel;

        public GenRandomParam grp;

        public int seed;

        public GradientList gradientList;

        public void Start() {
            // TODO MOVE IN LEVEL SCRIPT
            //GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
            //ShopManager.instance.UpdateGold();
            //UpgradeManager.instance.UpdateGold();
            Player player = GameSingleton.Instance.GetPlayer();
            if (player.gamemode == Player.Gamemode.ARCADE && player.currentScore == 0) {
                levelList.ClearLevels();
                player.currentLevelArcade = 0;
                GameSingleton.Instance.GetPlayer().beginGame = DateTime.Now;

                GenerateLevel();
            } else if (player.gamemode == Player.Gamemode.ARCADE) {
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevelArcade));
                var matGradPair = gradientList.GetRandomGradient();
                loadedLevel.terrainBuilder.heightGradient = matGradPair.gradient;
                loadedLevel.terrainBuilder.waterObject.GetComponent<MeshRenderer>().material = matGradPair.material;
                loadedLevel.Init();
            } else if (player.gamemode == Player.Gamemode.PERSONNALIZED) {
                String filename = GameSingleton.Instance.filename;
                Rule r = Rule.ReadLevel(filename.Split('.')[0]);
                player.arcadeGold = r.maxBudget;
                LoadLevel(r);
            } else {
                //GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(player.currentLevel));
                loadedLevel.Init();
                //player.goldStartLevel = player.gold;
                //player.inventoryStartLevel = player.storyModeInventory;  //TODO doit etre remis si pas fix
            }
        }

        public void NextLevel() {
            PoolManager.PoolManager.Instance().ReleaseAll();
            Player player = GameSingleton.Instance.GetPlayer();
            if (player.gamemode == Player.Gamemode.ARCADE) {
                Shop.Instance.ClearShop();

                player.currentLevelArcade =
                    (player.currentLevelArcade + 1) % levelList.LevelCount;
            }
            else if (player.gamemode == Player.Gamemode.PERSONNALIZED) {
                String filename = GameSingleton.Instance.filename;
                Rule r = Rule.ReadLevel(filename.Split('.')[0]);

                LoadLevel(r);
            }
            else {

                Shop.Instance.ClearShop();
                GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
                ShopManager.instance.UpdateGold();
                player.currentLevel =
                    (player.currentLevel + 1);// % levelList.LevelCount;
            }
        }

        public void SetLoadedLevel(Level level) {
            loadedLevel = level;
        }

        public void LoadLevel(Rule rule) {
            Shop.Instance.ClearShop();
            GameSingleton.Instance.GetPlayer().gold = rule.maxBudget;
            GameSingleton.Instance.GetPlayer().goldStartLevel = rule.maxBudget;
            GameSingleton.Instance.levelManager = this;

            Level levelNew = grp.levelBase;
            loadedLevel = Instantiate(levelNew);
            loadedLevel.rule = rule;
            loadedLevel.Init();

            GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
            ShopManager.instance.UpdateGold();
        }

        public void GenerateLevel() {
            //GameSingleton.Instance.GetPlayer().goldStartLevel = GameSingleton.Instance.GetPlayer().gold;
            //GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
            //update gold inventoryUI
            Player player = GameSingleton.Instance.GetPlayer();
            Debug.Log("BEFORE current level arcade is : " + GameSingleton.Instance.GetPlayer().currentLevelArcade);
            player.currentLevelArcade = player.currentLevelArcade + 1;
            Debug.Log("AFTER current level arcade is : " + GameSingleton.Instance.GetPlayer().currentLevelArcade);

            if (player.currentLevelArcade == 1) {

                seed = Random.Range(Int32.MinValue, Int32.MaxValue);
                player.currentSeed = seed;
            }

            Debug.Log("i="+player.currentLevelArcade);
            loadedLevel = grp.GenerateNextLevel(seed, player.currentLevelArcade);

            List<EnemySpawn> enemySpawns = loadedLevel.enemySpawns;

            grp.setDefaultGold(loadedLevel);
            GameSingleton.Instance.levelManager = this;
            
            loadedLevel = Instantiate(levelList.GetLevel(player.currentLevelArcade - 1));
            var matGradPair = gradientList.GetRandomGradient();
            loadedLevel.terrainBuilder.heightGradient = matGradPair.gradient;
            loadedLevel.terrainBuilder.waterObject.GetComponent<MeshRenderer>().material = matGradPair.material;
            loadedLevel.Init();
            /* foreach (var enemy in  enemySpawns)
             {
                 Vector2 spawnPosition = enemy.position;
 
                 float yPosition = loadedLevel.terrainBuilder.terrainOptions.modifierHeightMap[spawnPosition];
                 Debug.Log("position :" + yPosition);
                 Debug.Log(enemy.position);
             }*/
        }
    }
}