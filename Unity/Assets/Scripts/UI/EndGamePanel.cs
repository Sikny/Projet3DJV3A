using System.Collections.Generic;
using Game;
using Items;
using TMPro;
using Units;
using UnityEngine;
using Utility;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UI {
    public class EndGamePanel : MonoBehaviour {
        public TextMeshProUGUI winMessage;
        public TextMeshProUGUI loseMessage;
        public GameObject retryBtn;
        public GameObject nextBtn;
        public GameObject quitBtn;
        public GameObject finishStoryModePanel;
        public TextMeshProUGUI endMessage;
        public SystemUnit systemUnit; 
        public EndGame endGame; // arcade

        public int TypeEndGame {
            set {
                _typeEndGame = value;
                switch (_typeEndGame) {
                    case 0: // Lose
                        GameSingleton.Instance.soundManager.StopPlayingAllSounds();
                        GameSingleton.Instance.soundManager.Play("Defeat");
                        if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL) {
                            winMessage.gameObject.SetActive(false);
                            loseMessage.gameObject.SetActive(true);
                            retryBtn.SetActive(true);
                            quitBtn.SetActive(true);
                            nextBtn.SetActive(false);
                        }
                        else if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.PERSONNALIZED) {
                            winMessage.gameObject.SetActive(false);
                            loseMessage.gameObject.SetActive(true);
                            retryBtn.SetActive(true);
                            quitBtn.SetActive(true);
                            nextBtn.SetActive(false);
                        }
                        else { // Arcade
                            winMessage.gameObject.SetActive(false);
                            loseMessage.gameObject.SetActive(false);
                            retryBtn.SetActive(false);
                            nextBtn.SetActive(false);
                            quitBtn.SetActive(false);
                            endGame.gameObject.SetActive(true);
                            retryBtn.SetActive(false);
                            GameSingleton.Instance.GetPlayer().arcadeGold = 150;
                            GameSingleton.Instance.GetPlayer().arcadeModeInventory.Clear();
                            Shop.Instance.ClearShop();
                        }

                        break;
                    case 1: // Win
                        GameSingleton.Instance.soundManager.StopPlayingAllSounds();
                        GameSingleton.Instance.soundManager.Play("Victory");
                        if (GameSingleton.Instance.levelManager != null) {
                            UnitRecovery();
                            GameSingleton.Instance.levelManager.NextLevel();
                        }
                        Player plr = GameSingleton.Instance.GetPlayer();
                        Debug.Log("current level is : " + plr.currentLevel);
                        Debug.Log("max level is : " + GameSingleton.Instance.levelManager.levelList.LevelCount);
                        if (plr.currentLevel >= GameSingleton.Instance.levelManager.levelList.LevelCount)
                        {
                            winMessage.gameObject.SetActive(false);
                            loseMessage.gameObject.SetActive(false);
                            quitBtn.SetActive(true);
                            nextBtn.SetActive(false);
                            finishStoryModePanel.SetActive(true);
                            plr.currentLevel = 0;
                            plr.gold = 20;
                            plr.storyModeInventory.Clear();
                            plr.inventoryBackup.Clear();
                            plr.goldStartLevel = 0;
                        }
                        else
                        {
                            winMessage.gameObject.SetActive(true);
                            loseMessage.gameObject.SetActive(false);
                            retryBtn.SetActive(false);
                            quitBtn.SetActive(true);
                            nextBtn.SetActive(true);
                        }
                       

           

                      

                        break;
                    case 2: //Retry
                        winMessage.gameObject.SetActive(false);
                        loseMessage.gameObject.SetActive(false);
                        retryBtn.SetActive(false);
                        quitBtn.SetActive(false);
                        nextBtn.SetActive(false);

                        Player player = GameSingleton.Instance.GetPlayer();
                        if (player.gamemode == Player.Gamemode.LEVEL)
                        {
                            player.storyModeInventory = player.inventoryBackup;
                            player.gold = player.goldStartLevel;
                        }

                        break;
                }
            }
        }

        private int _typeEndGame;

        public void CallBtn(int idBtn) {
            //GameSingleton.Instance.GetPlayer().Save();
            switch (idBtn) {
                case 0:
                    GameSingleton.Instance.sceneManager.LoadScene("Menu");
                    break;
                case 1:

                    Player player = GameSingleton.Instance.GetPlayer();
                    Player.Gamemode playerGamemode = player.gamemode;


                    if (playerGamemode == Player.Gamemode.LEVEL) {
                       
                        player.gold = player.goldStartLevel;
                        player.storyModeInventory = player.inventoryBackup;
                    }
                    else if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE) {
                        Shop.Instance.ClearShop();
                        GameSingleton.Instance.levelManager.GenerateLevel();
                    }

                    Shop.Instance.ClearShop();
                    GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;
            }

            gameObject.SetActive(false);
        }

        private void UnitRecovery() {
//            SystemUnit 
            systemUnit = FindObjectOfType<SystemUnit>();

            List<AbstractUnit> units = systemUnit.GetUnits();
            
            Player player = GameSingleton.Instance.GetPlayer();
            
            Inventory inventory = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().storyModeInventory
                : GameSingleton.Instance.GetPlayer().arcadeModeInventory;
            foreach (var unit in units) {
                EntityType entityType = unit.GetEntityType();
                StoreUnit storeUnit = GameSingleton.Instance.storeUnitList.GetStoreUnitByEntityType(entityType);
                inventory.AddItem(storeUnit);
                if (player.gamemode == Player.Gamemode.LEVEL)
                {
                    player.BackupInventory(inventory);
                    player.goldStartLevel = player.gold;
                }

            }



        }
    }
}