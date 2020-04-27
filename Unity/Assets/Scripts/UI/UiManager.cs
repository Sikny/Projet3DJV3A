using Game;
using UnityEngine;
using Utility;

namespace UI {
    public class UiManager : MonoBehaviour {
        public GameObject shopPanel;
        public GameObject inventoryPanel;

        public InventoryManager inventoryUi;
        public Inventory inventory;

        private void Start() {
            GameSingleton.Instance.uiManager = this;
            inventory.Load(inventoryUi);
        }

        public void ToggleInventory() {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            shopPanel.SetActive(false);
        }

        public void ToggleShop() {
            shopPanel.SetActive(!shopPanel.activeSelf);
            inventoryPanel.SetActive(false);
        }

        public void HideUis() {
            inventoryPanel.SetActive(false);
            shopPanel.SetActive(false);
        }
    }
}
