using System.Collections.Generic;
using Items;
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
                Debug.Log("Several instances");
                return;
            }

            instance = this;
        }

        #endregion

        void Start() {
            _shop = Shop.Instance;

            UpdateUIItems();
            UpdateUIEquipments();
            UpdateUIUnits();
            UpdateGold();
            shopPanel.SetActive(false);
        }

        private void UpdateUIItems() {
            List<Consumable> shopConsummables = _shop.shopConsummables;
            
            for (int i = 0; i < shopConsummables.Count; i++) {
                ItemSlot currentSlot = prefabSlot;

                currentSlot.item = shopConsummables[i];
                currentSlot.itemName.SetText(shopConsummables[i].itemName);
                currentSlot.icon.sprite = shopConsummables[i].icon;
                currentSlot.price.SetText(shopConsummables[i].price.ToString());

                Instantiate(currentSlot, itemsParent, false);
            }
        }

        private void UpdateUIEquipments() {
            List<Equipment> shopEquipments = _shop.shopEquipments;

            for (int i = 0; i < shopEquipments.Count; i++) {
                ItemSlot currentSlot = prefabSlot;
                currentSlot.item = shopEquipments[i];
                currentSlot.itemName.SetText(shopEquipments[i].itemName);
                currentSlot.icon.sprite = shopEquipments[i].icon;

                Instantiate(currentSlot, equipmentsParent, false);
            }
        }

        private void UpdateUIUnits() {
            List<StoreUnit> shopUnits = _shop.shopUnits;

            for (int i = 0; i < shopUnits.Count; i++) {
                ItemSlot currentSlot = prefabSlot;
                currentSlot.item = shopUnits[i];
                currentSlot.itemName.SetText(shopUnits[i].itemName);
                currentSlot.icon.sprite = shopUnits[i].icon;

                Instantiate(currentSlot, unitsParent, false);
            }
        }

        public void UpdateGold() {
            goldText.SetText(GameSingleton.Instance.GetPlayer().GetGold() + "g");
        }

        public GameObject fightButton;

        public void Fight() {
            int playerCount = FindObjectsOfType<PlayerUnit>().Length;
            if (playerCount == 0) return;
            systemUnit.SetRunning(true);
            fightButton.SetActive(false);
            shopPanel.SetActive(false);
            GameSingleton.Instance.StartFight();
        }
    }
}