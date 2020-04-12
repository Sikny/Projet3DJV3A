using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;

namespace UI {
    public class InventoryManager : ItemsPanel {
        public TextMeshProUGUI goldText;

        public GameObject inventoryPanel;

        public ItemSlot prefabSlot;

        private List<ItemSlot> _consummableSlots = new List<ItemSlot>();

        private List<ItemSlot> _unitSlots = new List<ItemSlot>();

        #region Singleton

        public static InventoryManager instance;

        private void Awake() {
            if (instance != null) {
                Debug.Log("Several instances");
                return;
            }

            instance = this;
        }

        #endregion

        // Start is called before the first frame update
        void Start() {
            inventoryPanel.SetActive(false);
            UpdateGold();
        }

        public void UpdateUiConsumable(Consumable consumable) {
            ItemSlot currentSlot = prefabSlot;

            currentSlot.item = consumable;
            currentSlot.itemName.SetText(consumable.itemName);
            currentSlot.icon.sprite = consumable.icon;

            _consummableSlots.Add(Instantiate(currentSlot, itemsParent, false));
        }

        public void UpdateUiEquipment(Equipment equipment) {
            ItemSlot currentSlot = prefabSlot;

            currentSlot.item = equipment;
            currentSlot.itemName.SetText(equipment.itemName);
            currentSlot.icon.sprite = equipment.icon;

            Instantiate(currentSlot, equipmentsParent, false);
        }

        public void UpdateUiUnit(StoreUnit unit) {
            ItemSlot currentSlot = prefabSlot;

            currentSlot.item = unit;
            currentSlot.itemName.SetText(unit.itemName);
            currentSlot.icon.sprite = unit.icon;

            _unitSlots.Add(Instantiate(currentSlot, unitsParent, false));
        }

        public void RemoveConsumable(Consumable consumable) {
            int targetIndex = 0;
            for (int i = _consummableSlots.Count - 1; i > 0; i--) {
                if (_consummableSlots[i].item == consumable) {
                    targetIndex = i;
                    break;
                }
            }

            GameObject slotGameobject = _consummableSlots[targetIndex].gameObject;
            Destroy(slotGameobject);
            _consummableSlots.Remove(_consummableSlots[targetIndex]);
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
            goldText.SetText(Player.instance.gold + " g");
        }
    }
}