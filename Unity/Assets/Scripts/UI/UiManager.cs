using Game;
using TMPro;
using UnityEngine;
using Utility;

namespace UI {
    public class UiManager : MonoBehaviour {
        public GameObject shopPanel;
        public GameObject inventoryPanel;
        public GameObject upgradePanel;
        public GameObject descriptionBox;
        public TextMeshProUGUI description;

        public InventoryManager inventoryUi;
        public Inventory inventory;

        //public UpgradeManager upgradeUI; TODO
        
        private void Start() {
            GameSingleton.Instance.uiManager = this;
            inventory.Load(inventoryUi);
        }

        public void ToggleInventory() {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            shopPanel.SetActive(false);
            descriptionBox.SetActive(false);
        }

        public void ToggleShop()
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
            inventoryPanel.SetActive(false);
            descriptionBox.SetActive(false);
        }

        public void ToggleUpgradePanel()
        {
            upgradePanel.SetActive(!upgradePanel.activeSelf);
            descriptionBox.SetActive(false);
        }

        public void HideUis() {
            descriptionBox.SetActive(false);
            inventoryPanel.SetActive(false);
            shopPanel.SetActive(false);
        }
    }
}
