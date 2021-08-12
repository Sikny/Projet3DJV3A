using Game;
using Utility;

namespace Items {
    public class Consumable : Item
    {
        //public String description = ""; 

        public override void Use()
        {
            base.Use();
            GameSingleton.Instance.soundManager.Play("ItemUse");
            RemoveFromInventory();
            GameSingleton.Instance.uiManager.descriptionBox.SetActive(false);

        }
    
        public void RemoveFromInventory ()
        {
            Inventory inventory = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().storyModeInventory
                : GameSingleton.Instance.GetPlayer().arcadeModeInventory;
            inventory.RemoveConsumable(this);
        }
    }
}