using System.Collections.Generic;

namespace Items {
    public class Shop
    {
        public List<Consumable> shopConsummables = new List<Consumable>();
        public List<Equipment> shopEquipments = new List<Equipment>();
        public List<StoreUnit> shopUnits = new List<StoreUnit>();

        #region Singleton
    
        private static Shop _instance;

        public static Shop Instance {
            get {
                if(_instance == null) _instance = new Shop();
                return _instance;
            }
        }

        #endregion

        public void AddConsummable(Consumable item)
        {
            shopConsummables.Add(item);
        }

        public void AddEquipment(Equipment equipment)
        {
            shopEquipments.Add(equipment);
        }

        public void AddStoreUnit(StoreUnit unit) {
            shopUnits.Add(unit);
        }

        public void ClearShop() {
            shopConsummables.Clear();
            shopEquipments.Clear();
            shopUnits.Clear();
        }
    }
}
