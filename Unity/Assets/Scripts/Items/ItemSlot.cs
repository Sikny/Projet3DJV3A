using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items {
    public class ItemSlot : MonoBehaviour
    {
    
        public Image icon;

        public TextMeshProUGUI itemName;

        public TextMeshProUGUI price;

        public ItemSlot inventoryPrefab;

        private Popups _popup;

        //-------------------------
 
        public Item item;

        private Player _player;

        private Inventory _inventory;
        //private ShopManager _shopManager;

        public void Start()
        {
            _player = Player.instance;
            _inventory = Inventory.instance;
            _popup = Popups.instance;

        }

        public void BuyItem()
        {
            if (_player.gold >= item.price)
            {
                _player.gold -= item.price;
                ItemSlot boughtSlot = inventoryPrefab;
                boughtSlot.item = item;
                boughtSlot.icon.sprite = icon.sprite;
                boughtSlot.itemName.SetText(itemName.text);
                _inventory.AddItem(boughtSlot.item);
                //Instantiate(boughtSlot, equipmentsParent, false);

                //Player.instance.inventory.add(item);
            }
            else
            {
                _popup.Popup("not enough gold!");
            }
        
        }


        public void AddItem(Item newItem)
        {
            item = newItem;
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        public void RemoveItem()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
        }


        public void UseItem()
        {
            item.Use();
        }

    }
}
