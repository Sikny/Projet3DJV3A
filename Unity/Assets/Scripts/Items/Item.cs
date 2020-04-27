using UI;
using UnityEngine;

namespace Items {
    public class Item : ScriptableObject {
        public string itemName = "New item";
        public Sprite icon;

        public int price = 10;
        //public String description = ""; 


        // Called when the item is pressed in the inventory
        public virtual void Use() {
            Popups.instance.Popup("using " + itemName);
        }
    }
}