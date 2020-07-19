using System.Collections.Generic;
using Game;
using Items;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.WSA;
using Utility;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UI {
    public class EndGamePanel : MonoBehaviour {
        public TextMeshProUGUI winMessage;
        public TextMeshProUGUI loseMessage;
        public GameObject retryBtn;
        public GameObject nextBtn;
        public GameObject quitBtn;
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
                        winMessage.gameObject.SetActive(true);
                        loseMessage.gameObject.SetActive(false);
                        retryBtn.SetActive(false);
                        quitBtn.SetActive(true);
                        nextBtn.SetActive(true);

                        /*if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE) {
                            Shop.Instance.ClearShop();
                            //GameSingleton.Instance.levelManager.GenerateLevel();
                        }*/

                        if (GameSingleton.Instance.levelManager != null) {
                            UnitRecovery();
                            GameSingleton.Instance.levelManager.NextLevel();
                        }

                        break;
                    case 2: //Retry

                        Player player = GameSingleton.Instance.GetPlayer();
                        player.SetGold();
                        player.SetInventory();
                        winMessage.gameObject.SetActive(false);
                        loseMessage.gameObject.SetActive(false);
                        retryBtn.SetActive(false);
                        quitBtn.SetActive(false);
                        nextBtn.SetActive(false);

                        /*
                        Player player = GameSingleton.Instance.GetPlayer();
                        if (player.gamemode == Player.Gamemode.LEVEL)
                        {
                            player.storyModeInventory = player.inventoryStartLevel;
                            player.gold = player.goldStartLevel;
                        }*/

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
                    
                    if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE) {
                        Shop.Instance.ClearShop();
                        GameSingleton.Instance.levelManager.GenerateLevel();
                    }
                    /*if (playerGamemode == Player.Gamemode.LEVEL) {
                        player.gold = player.goldStartLevel;
                        player.storyModeInventory = player.inventoryStartLevel;
                    }*/

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
            }
     




        }
    }
}