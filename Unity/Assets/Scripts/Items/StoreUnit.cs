using System.Collections.Generic;
using UI;
using Units;
using UnityEngine;
using Utility;

namespace Items {
    [CreateAssetMenu(fileName = "New Unit", menuName = "ScriptableObject/Unit")]
    public class StoreUnit : Item {
        public EntityType entityType;
        //public List<EntityType> upgrades;
        public List<StoreUnit> upgrades;
        
        public override void Use() {
            base.Use();

            GameSingleton.Instance.uiManager.HideUis();
            ShopManager.instance.gameObject.SetActive(false);
            GameSingleton.Instance.uiManager.inventory.selectedStoreUnit = this;
            
            Popups.instance.popupText.text = "Placing " + itemName;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            UpgradeManager.instance.SetUIUnits(this);
            //UpgradeManager.instance.SetUIUnits(entityType);
            //EntityDict.GetEntityType(entityType);
            //UpgradeManager.instance.SetUIUnits(item);
            //activate other UI and deactivate others
            //set unit selected
            //get units upgradable ones 
        }

    }
}