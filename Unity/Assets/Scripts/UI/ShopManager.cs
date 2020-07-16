using System.Collections.Generic;
using Game;
using Items;
using Language;
using TMPro;
using Units;
using UnityEngine;
using Utility;

namespace UI {
    public class ShopManager : ItemsPanel {
        public TextMeshProUGUI goldText;

        public GameObject shopPanel;

        public ItemSlot prefabSlot;

        private Shop _shop;


        #region Singleton

        public static ShopManager instance;


        private void Awake() {
            if (instance != null) {
                return;
            }

            instance = this;
        }

        #endregion

        void Start() {
            _shop = Shop.Instance;

            UpdateUiItems();
            UpdateUiEquipments();
            UpdateUiUnits();
            UpdateGold();
            //shopPanel.SetActive(false);
        }

        public void UpdateUi(Shop shop) {
            _shop = shop;
            UpdateUiItems();
            UpdateUiEquipments();
            UpdateUiUnits();
            UpdateGold();
        }

        private void UpdateUiItems() {
            List<Consumable> shopConsummables = _shop.shopConsummables;

            for (int i = 0; i < shopConsummables.Count; i++) {
                ItemSlot addedItem = Instantiate(prefabSlot, itemsParent, false);
                addedItem.item = shopConsummables[i];
                addedItem.itemName.SetText(Traducer.Translate(shopConsummables[i].itemName));
                addedItem.icon.sprite = shopConsummables[i].icon;
                addedItem.price.SetText(shopConsummables[i].price.ToString());
            }
        }

        private void UpdateUiEquipments() {
            List<Equipment> shopEquipments = _shop.shopEquipments;

            for (int i = 0; i < shopEquipments.Count; i++) {
                ItemSlot addedItem = Instantiate(prefabSlot, equipmentsParent, false);
                addedItem.item = shopEquipments[i];
                addedItem.itemName.SetText(Traducer.Translate(shopEquipments[i].itemName));
                addedItem.icon.sprite = shopEquipments[i].icon;
                addedItem.price.SetText(shopEquipments[i].price.ToString());
            }
        }

        private void UpdateUiUnits() {
            List<StoreUnit> shopUnits = _shop.shopUnits;

            for (int i = 0; i < shopUnits.Count; i++) {
                ItemSlot addedItem = Instantiate(prefabSlot, unitsParent, false);
                addedItem.item = shopUnits[i];
                addedItem.itemName.SetText(Traducer.Translate(shopUnits[i].itemName));
                addedItem.icon.sprite = shopUnits[i].icon;
                addedItem.price.SetText(shopUnits[i].price.ToString());
            }
        }

        public void UpdateGold() {
            int gold = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().gold
                : GameSingleton.Instance.GetPlayer().arcadeGold;
            goldText.SetText(gold + " g");
        }

        public GameObject fightButton;

        public void Fight() {
            int playerCount = FindObjectsOfType<PlayerUnit>().Length;
            if (playerCount == 0) {
                Popups.instance.Popup("At least one unit must be placed first", Color.red);
                return;
            }

            systemUnit.SetRunning(true);
            fightButton.SetActive(false);
            shopPanel.SetActive(false);
            GameSingleton.Instance.StartFight();
        }
    }
}