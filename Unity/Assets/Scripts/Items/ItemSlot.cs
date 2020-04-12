using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Items {
    public class ItemSlot : MonoBehaviour {
        public Image icon;

        public TextMeshProUGUI itemName;

        public TextMeshProUGUI price;

        public ItemSlot inventoryPrefab;

        private Popups _popup;

        public Item item;

        private Player _player;
        private Inventory _inventory;

        public void Start() {
            _player = GameSingleton.Instance.GetPlayer();
            _inventory = GameSingleton.Instance.uiManager.inventory;
            _popup = Popups.instance;
        }

        public void BuyItem() {
            if (_player.gold >= item.price) {
                _player.gold -= item.price;
                ItemSlot boughtSlot = inventoryPrefab;
                boughtSlot.item = item;
                boughtSlot.icon.sprite = icon.sprite;
                boughtSlot.itemName.SetText(itemName.text);
                _inventory.AddItem(boughtSlot.item);
            }
            else {
                _popup.Popup("Not enough gold !");
            }
        }

        public void UseItem() {
            item.Use();
        }
    }
}