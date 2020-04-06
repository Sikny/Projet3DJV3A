using Units;
using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "New Unit", menuName = "ScriptableObjects/Unit")]
    public class StoreUnit : Item {
        public EntityType entityType;
        
        public override void Use() {
            base.Use();

            InventoryContent.instance.selectedStoreUnit = this;
            
            Popups.instance.popupText.text = "Placing " + name;
        }
    }
}