using System.Collections.Generic;
using Items;
using UI;
using UnityEngine;

namespace Game {
    public class Inventory : MonoBehaviour {
        public List<Consumable> inventoryConsummables = new List<Consumable>();
        public List<Equipment> inventoryEquipments = new List<Equipment>();
        public List<StoreUnit> inventoryUnits = new List<StoreUnit>();

        public StoreUnit selectedStoreUnit;

        private InventoryManager _inventoryManager;
        private ShopManager _shopManager;

        #region Singleton

        public static Inventory instance;


        private void Awake() {
            if (instance != null) {
                Debug.Log("Several instances");
                return;
            }
            instance = this;
        }

        #endregion


        private void Start() {
            _inventoryManager = InventoryManager.instance;
            _shopManager = ShopManager.instance;

            foreach (var unit in inventoryUnits) {
                _inventoryManager.UpdateUiUnit(unit);
            }

            foreach (var cons in inventoryConsummables) {
                _inventoryManager.UpdateUiConsumable(cons);
            }

            foreach (var equips in inventoryEquipments) {
                _inventoryManager.UpdateUiEquipment(equips);
            }
        }

        public void AddItem(Item item) {
            _inventoryManager.UpdateGold();
            _shopManager.UpdateGold();
            var itemType = item.GetType();

            if (itemType.IsSubclassOf(typeof(Equipment)) || itemType == typeof(Equipment)) {
                AddEquipment((Equipment) item);
            }
            else if (itemType.IsSubclassOf(typeof(Consumable)) || itemType == typeof(Consumable)) {
                AddConsumable((Consumable) item);
            }
            else if (itemType.IsSubclassOf(typeof(StoreUnit)) || itemType == typeof(StoreUnit)) {
                AddUnit((StoreUnit) item);
            }
        }

        private void AddConsumable(Consumable item) {
            inventoryConsummables.Add(item);
            _inventoryManager.UpdateUiConsumable(item);
        }

        private void AddEquipment(Equipment equipment) {
            inventoryEquipments.Add(equipment);
            _inventoryManager.UpdateUiEquipment(equipment);
        }

        private void AddUnit(StoreUnit unit) {
            inventoryUnits.Add(unit);
            _inventoryManager.UpdateUiUnit(unit);
        }


        public void RemoveConsumable(Consumable item) {
            inventoryConsummables.Remove(item);
            _inventoryManager.RemoveConsumable(item);
        }

        public void RemoveUnit(StoreUnit unit) {
            inventoryUnits.Remove(unit);
            _inventoryManager.RemoveUnit(unit);
        }
    }
}