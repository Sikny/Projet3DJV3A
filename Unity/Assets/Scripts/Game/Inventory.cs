using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Game {
    public class Inventory : MonoBehaviour
    {

        public List<ItemSlot> itemSlotsConsumable = new List<ItemSlot>();

        public List<Consumable> inventoryConsummables = new List<Consumable>();
        public List<Equipment> inventoryEquipments = new List<Equipment>();
        public List<StoreUnit> inventoryUnits = new List<StoreUnit>();

        public StoreUnit selectedStoreUnit;

        private InventoryManager _inventoryManager;
        private ShopManager _shopManager;
        #region Singleton
    
        public static Inventory instance;
    
    
        private void Awake()
        {
            if (instance != null)
            {
                Debug.Log("Several instances");
                return;
            }
            instance = this;
        }

        #endregion


        private void Start()
        {
            _inventoryManager = InventoryManager.instance;
            _shopManager = ShopManager.instance;

            foreach (var unit in inventoryUnits) {
                _inventoryManager.UpdateUIUnit(unit);
            }
            foreach (var cons in inventoryConsummables) {
                _inventoryManager.UpdateUIConsummable(cons);
            }
            foreach (var equips in inventoryEquipments) {
                _inventoryManager.UpdateUIEquipment(equips);
            }
        }

        public void ClearAllItems()
        {
            itemSlotsConsumable.Clear();
        
        }

        public void FillItemSlot(Item newItem)
        {
            int nextIndex = itemSlotsConsumable.Count + 1;
            itemSlotsConsumable[nextIndex].AddItem(newItem);
        

        }

        public void AddItem(Item item)
        {
            _inventoryManager.UpdateGold();
            _shopManager.UpdateGold();
            var itemType = item.GetType();
        
            if (itemType.IsSubclassOf(typeof(Equipment)) || itemType == typeof(Equipment))
            {
                AddEquipment((Equipment)item);
            }
            else if(itemType.IsSubclassOf(typeof(Consumable)) || itemType == typeof(Consumable))
            {
                AddConsummable((Consumable)item);
            }
            else if (itemType.IsSubclassOf(typeof(StoreUnit)) || itemType == typeof(StoreUnit)) {
                AddUnit((StoreUnit) item);
            }
        }

        private void AddConsummable(Consumable item)
        {
            inventoryConsummables.Add(item);
            _inventoryManager.UpdateUIConsummable(item);
        }

        private void AddEquipment(Equipment equipment)
        {
            inventoryEquipments.Add(equipment);
            _inventoryManager.UpdateUIEquipment(equipment);
        }

        private void AddUnit(StoreUnit unit) {
            inventoryUnits.Add(unit);
            _inventoryManager.UpdateUIUnit(unit);
        }


        public void RemoveConsummable(Consumable item)
        {
            inventoryConsummables.Remove(item);
            _inventoryManager.RemoveConsummable(item);
        }

        public void RemoveUnit(StoreUnit unit) {
            inventoryUnits.Remove(unit);
            _inventoryManager.RemoveUnit(unit);
        }

        public void AddAllConsummables()
        {
        
            List<Consumable>allConsummables = StoreItems.instance.GetAllItems();

            for(int i = 0; i < allConsummables.Count - 1; i++)
            {
                inventoryConsummables.Add(allConsummables[i]);
            }
        }

        public void AddAllEquipments()
        {
            List<Equipment>allEquipments = StoreItems.instance.GetAllEquipments();

            for(int i = 0; i < allEquipments.Count - 1; i++)
            {
                inventoryEquipments.Add(allEquipments[i]);
            }
        }
    
        public List<Consumable> GetInventoryConsummables()
        {
            return inventoryConsummables;
        }

        public List<Equipment> GetInventoryEquipments()
        {
            return inventoryEquipments;
        }

        public List<StoreUnit> GetInventoryUnits() {
            return inventoryUnits;
        }
    }
}
