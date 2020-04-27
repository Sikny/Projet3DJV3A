using Game;
using Items;
using UnityEditor;
using UnityEngine;

namespace Editor {
    public class PlayerTools : MonoBehaviour {
        private static Inventory _inventory;
        private static StoreUnit[] _availableUnits;
        private static Consumable[] _availableConsumables;
        private static Equipment[] _availableEquipments;
        
        

        [MenuItem("Tools/Clear Player Data")]
        public static void ClearPlayerData() {
            InitData();
            PlayerPrefs.DeleteAll();
            _inventory.Clear();
        }

        [MenuItem("Tools/Set Player Data As Default")]
        public static void ResetDefaultPlayerData() {
            InitData();
            ClearPlayerData();
            var soldierItem = FindUnitByName(_availableUnits, "Soldier");
            // Begin with 2 soldiers
            if (soldierItem == null) return;
            _inventory.units.Add(soldierItem);
            _inventory.units.Add(soldierItem);
        }

        private static void InitData() {
            _inventory = Resources.FindObjectsOfTypeAll<Inventory>()[0];
            _availableUnits = Resources.FindObjectsOfTypeAll<StoreUnit>();
            _availableConsumables = Resources.FindObjectsOfTypeAll<Consumable>();
            _availableEquipments = Resources.FindObjectsOfTypeAll<Equipment>();
        }

        private static StoreUnit FindUnitByName(StoreUnit[] array, string itemName) {
            foreach (var unit in array) {
                if (unit.itemName == itemName)
                    return unit;
            }
            return null;
        }
    }
}
