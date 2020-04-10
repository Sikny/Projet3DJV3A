using Game;
using UI;
using Units;
using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "New Unit", menuName = "ScriptableObject/Unit")]
    public class StoreUnit : Item {
        public EntityType entityType;
        
        public override void Use() {
            base.Use();

            InventoryManager.instance.gameObject.SetActive(false);
            ShopManager.instance.gameObject.SetActive(false);
            Inventory.instance.selectedStoreUnit = this;
            
            Popups.instance.popupText.text = "Placing " + itemName;
        }
    }
}