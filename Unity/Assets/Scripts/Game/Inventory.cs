using System.Collections.Generic;
using Items;
using UI;
using UnityEngine;

namespace Game {
    [CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObject/Inventory")]
    public class Inventory : ScriptableObject{
        public List<Consumable> consumables = new List<Consumable>();
        public List<Equipment> equipments = new List<Equipment>();
        public List<StoreUnit> units = new List<StoreUnit>();

        public StoreUnit selectedStoreUnit;

        private InventoryManager _inventoryManager;
        private ShopManager _shopManager;
        
        public void Load(InventoryManager inventoryUi) {
            _inventoryManager = inventoryUi;
            _shopManager = ShopManager.instance;

            foreach (var unit in units) {
                _inventoryManager.UpdateUiUnit(unit);
            }

            foreach (var cons in consumables) {
                _inventoryManager.UpdateUiConsumable(cons);
            }

            foreach (var equips in equipments) {
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
            consumables.Add(item);
            _inventoryManager.UpdateUiConsumable(item);
        }

        private void AddEquipment(Equipment equipment) {
            equipments.Add(equipment);
            _inventoryManager.UpdateUiEquipment(equipment);
        }

        private void AddUnit(StoreUnit unit) {
            units.Add(unit);
            _inventoryManager.UpdateUiUnit(unit);
        }


        public void RemoveConsumable(Consumable item) {
            consumables.Remove(item);
            _inventoryManager.RemoveConsumable(item);
        }

        public void RemoveUnit(StoreUnit unit) {
            units.Remove(unit);
            _inventoryManager.RemoveUnit(unit);
        }

        public void Clear() {
            units.Clear();
            consumables.Clear();
            equipments.Clear();
        }
    }
}