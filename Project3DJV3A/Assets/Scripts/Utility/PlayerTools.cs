using Game;
using Items;
using UnityEngine;

namespace Editor {
    [CreateAssetMenu(fileName = "PlayerTools", menuName = "ScriptableObject/PlayerTools")]
    public class PlayerTools : ScriptableObject {
        [Header("ScriptableObjects")]
        [SerializeField] private Inventory storyModeInventory;

        [Header("Default Player Inventory")]
        [SerializeField] private int goldAmount;    // TODO MOVE IN INVENTORY
        [SerializeField] private StoreUnit[] availableUnits;
        [SerializeField] private Consumable[] availableConsumables;

        // TODO
        
        [ContextMenu("Set Player Data As Default")]
        public void ResetDefaultPlayerData() {
            
            PlayerPrefs.DeleteAll();
            storyModeInventory.Clear();
            
            for (int i = availableUnits.Length - 1; i >= 0; i--) {
                storyModeInventory.AddItem(availableUnits[i]);
            }
            for (int i = availableConsumables.Length - 1; i >= 0; i--) {
                storyModeInventory.AddItem(availableConsumables[i]);
            }
        }
    }
}
