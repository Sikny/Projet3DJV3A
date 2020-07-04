using System.Collections.Generic;
using Game;
using Items;
using TMPro;
using UnityEngine;
using Utility;

namespace UI {
    public class InventoryManager : ItemsPanel {
        public TextMeshProUGUI goldText;

        public GameObject inventoryPanel;

        public ItemSlot prefabSlot;

        public ItemSlot unitPrefabSlot;

        private List<ItemSlot> _consumableSlots = new List<ItemSlot>();

        private List<ItemSlot> _unitSlots = new List<ItemSlot>();

        void Start() {
            inventoryPanel.SetActive(false);
            UpdateGold();
        }

        public void UpdateUiConsumable(Consumable consumable) {
            var addedItem = Instantiate(prefabSlot, itemsParent, false);
            addedItem.item = consumable;
            addedItem.itemName.SetText(consumable.itemName);
            addedItem.icon.sprite = consumable.icon;

            _consumableSlots.Add(addedItem);
        }

        public void UpdateUiEquipment(Equipment equipment) {
            var addedItem = Instantiate(prefabSlot, equipmentsParent, false);
            addedItem.item = equipment;
            addedItem.itemName.SetText(equipment.itemName);
            addedItem.icon.sprite = equipment.icon;
        }

        public void UpdateUiUnit(StoreUnit unit)
        {

            ItemSlot targetSlot = (unit.upgrades.Count > 0) ? unitPrefabSlot : prefabSlot;
 
            var addedItem = Instantiate(targetSlot, unitsParent, false);
            
            addedItem.item = unit;
            addedItem.itemName.SetText(unit.itemName);
            addedItem.icon.sprite = unit.icon;
            
            _unitSlots.Add(addedItem);
        }

        public void RemoveConsumable(Consumable consumable) {
            int targetIndex = 0;
            for (int i = _consumableSlots.Count - 1; i > 0; i--) {
                if (_consumableSlots[i].item == consumable) {
                    targetIndex = i;
                    break;
                }
            }

            GameObject slotGameObject = _consumableSlots[targetIndex].gameObject;
            Destroy(slotGameObject);
            _consumableSlots.Remove(_consumableSlots[targetIndex]);
        }

        public void RemoveUnit(StoreUnit unit) {
            int targetIndex = 0;
            for (int i = _unitSlots.Count - 1; i > 0; i--) {
                if (_unitSlots[i].item == unit) {
                    targetIndex = i;
                    break;
                }
            }

            GameObject slotGameObject = _unitSlots[targetIndex].gameObject;
            Destroy(slotGameObject);
            _unitSlots.Remove(_unitSlots[targetIndex]);
        }

        public void UpdateGold() {
            int gold = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().gold
                : GameSingleton.Instance.GetPlayer().arcadeGold;
            goldText.SetText(gold + " g");
        }
    }
}