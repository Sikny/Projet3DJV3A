using Utility;

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
            GameSingleton.Instance.uiManager.inventory.RemoveConsumable(this);
        }
    }
}