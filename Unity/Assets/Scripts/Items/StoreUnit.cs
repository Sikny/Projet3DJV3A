using Units;
using UnityEditor;
using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "New Unit", menuName = "ScriptableObjects/Unit")]
    public class StoreUnit : Item {
        public EntityType entityType;
        
        public override void Use() {
            base.Use();

            InventoryManager.instance.gameObject.SetActive(false);
            ShopManager.instance.gameObject.SetActive(false);
            InventoryContent.instance.selectedStoreUnit = this;
            
            Popups.instance.popupText.text = "Placing " + name;
        }
    }
}