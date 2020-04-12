using Game;
using UnityEngine;

namespace Items {
    public class Consumable : Item
    {
        //public String description = ""; 

        public override void Use()
        {
            base.Use();
            RemoveFromInventory();
        }
    
        public void RemoveFromInventory ()
        {
            Inventory.instance.RemoveConsumable(this);
        }
    }
}