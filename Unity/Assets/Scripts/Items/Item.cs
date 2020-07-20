using Language;
using Sounds;
using UI;
using UnityEngine;
using Utility;

namespace Items {
    public class Item : ScriptableObject {
        public string itemName = "New item";
        public Sprite icon;

        
        public int price = 10;
        public string description = ""; 


        // Called when the item is pressed in the inventory
        public virtual void Use() {
            
            Popups.instance.Popup(Traducer.Translate("using ") + Traducer.Translate(itemName));
        }

        public virtual void Upgrade()
        {
            
        }
    }
}