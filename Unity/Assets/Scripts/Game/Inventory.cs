using System.Collections.Generic;
using Items;
using UI;
using UnityEngine;
using Utility;

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

            /*foreach (var unit in units) {
                _inventoryManager.UpdateUiUnit(unit);
            }

            foreach (var cons in consumables) {
                _inventoryManager.UpdateUiConsumable(cons);
            }

            foreach (var equips in equipments) {
                _inventoryManager.UpdateUiEquipment(equips);
            }*/
        }

        public void AddItem(Item item) {
            GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
            //_inventoryManager.UpdateGold();
            ShopManager.instance.UpdateGold();
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
            GameSingleton.Instance.uiManager.inventoryUi.UpdateUiConsumable(item);
        }

        private void AddEquipment(Equipment equipment) {
            equipments.Add(equipment);
            GameSingleton.Instance.uiManager.inventoryUi.UpdateUiEquipment(equipment);
        }

        private void AddUnit(StoreUnit unit) {
            units.Add(unit);
            GameSingleton.Instance.uiManager.inventoryUi.UpdateUiUnit(unit);
        }


        public void RemoveConsumable(Consumable item) {
            consumables.Remove(item);
            GameSingleton.Instance.uiManager.inventoryUi.RemoveConsumable(item);
        }

        public void RemoveUnit(StoreUnit unit) {
            units.Remove(unit);
            GameSingleton.Instance.uiManager.inventoryUi.RemoveUnit(unit);
        }

        public void Clear() {
            units.Clear();
            consumables.Clear();
            equipments.Clear();
            
            //GameSingleton.Instance.uiManager.inventoryUi.ClearUiInventory();

        }
    }
}