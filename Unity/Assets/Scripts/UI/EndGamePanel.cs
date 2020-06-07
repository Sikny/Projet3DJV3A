using System.Collections.Generic;
using Game;
using Items;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UI {
    public class EndGamePanel : MonoBehaviour {
        public TextMeshProUGUI winMessage;
        public TextMeshProUGUI loseMessage;
        public GameObject retryBtn;
        public GameObject nextBtn;
        public SystemUnit systemUnit;
        public int TypeEndGame {
            set {
                _typeEndGame = value;
                switch (_typeEndGame) {
                    case 0:    // Lose
                        winMessage.gameObject.SetActive(false);
                        loseMessage.gameObject.SetActive(true);
                        retryBtn.SetActive(true);
                        nextBtn.SetActive(false);
                        break;
                    case 1:    // Win
                        winMessage.gameObject.SetActive(true);
                        loseMessage.gameObject.SetActive(false);
                        retryBtn.SetActive(false);
                        nextBtn.SetActive(true);

                        if (GameSingleton.Instance.levelManager != null)
                        {
                            UnitRecovery();
                            GameSingleton.Instance.levelManager.NextLevel();

                        }
                        break;
                }
            }
        }
        private int _typeEndGame;

        public void CallBtn(int idBtn) {
            switch (idBtn) {
                case 0:
                    GameSingleton.Instance.sceneManager.LoadScene("Menu");
                    break;
                case 1:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;
            }
            gameObject.SetActive(false);
        }
        
        private void UnitRecovery()
        {
//            SystemUnit 
            systemUnit = FindObjectOfType<SystemUnit>();

            List<AbstractUnit> units = systemUnit.GetUnits();

            Inventory inventory = GameSingleton.Instance.inventory;

            foreach (var unit in units)
            {
                EntityType entityType = unit.GetEntityType();
                StoreUnit storeUnit = GameSingleton.Instance.storeUnitList.GetStoreUnitByEntityType(entityType);
                inventory.AddItem(storeUnit);
            }
        }
    }
    
}
