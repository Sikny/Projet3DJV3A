using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public class StoreItems : MonoBehaviour
    {
        public List<Consumable> allConssumables;
        public List<Equipment> allEquipments;

    
        #region Singleton
    
        public static StoreItems instance;
    
    
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
        public List<Consumable> GetAllItems()
        {
            return allConssumables;
        }

        public List<Equipment> GetAllEquipments()
        {
            return allEquipments;
        }


    
    }
}
