﻿using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI {
    public class UiManager : MonoBehaviour {
        public GameObject shopPanel;
        public GameObject inventoryPanel;
        public GameObject upgradePanel;
        public GameObject pausePanel;
        public GameObject descriptionBox;
        public TextMeshProUGUI description;
        public GameObject unitNameBox;
        public Image image;
        public TextMeshProUGUI unitName;
        public InventoryManager inventoryUi;
        private Inventory _inventory;

        //public UpgradeManager upgradeUI; TODO

        private void Start() {
            GameSingleton.Instance.uiManager = this;
            _inventory = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().storyModeInventory
                : GameSingleton.Instance.GetPlayer().arcadeModeInventory;
            _inventory.Load(inventoryUi);
            inventoryUi.Init();
        }

        public void ToggleInventory() {
            inventoryUi.UpdateGold();
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            //shopPanel.SetActive(false);
            //descriptionBox.SetActive(false);
        }

        public void ToggleShop() {
            ShopManager.instance.UpdateGold();
            shopPanel.SetActive(!shopPanel.activeSelf);
            //inventoryPanel.SetActive(false);
            //descriptionBox.SetActive(false);
        }

        public void ToggleUpgradePanel() {
            UpgradeManager.instance.UpdateGold();
            upgradePanel.SetActive(!upgradePanel.activeSelf);
            //descriptionBox.SetActive(false);
        }

        public void HideUis() {
            descriptionBox.SetActive(false);
            inventoryPanel.SetActive(false);
            shopPanel.SetActive(false);
        }
    }
}