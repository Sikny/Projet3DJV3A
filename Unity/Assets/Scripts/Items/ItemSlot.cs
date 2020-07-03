using Game;
using TMPro;
using UI;
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
            _inventory = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().storyModeInventory
                : GameSingleton.Instance.GetPlayer().arcadeModeInventory;
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
                _popup.Popup("Bought " + item.name + "!");
            }
            else
            {
                _popup.Popup("Not enough gold !", Color.red);
            }
        }

        public void UseItem() {
            item.Use();
            _popup.Popup("Used " + item.name + "!");
        }

        public void UpgradeUnit()
        {
            item.Upgrade();

        }
    }
}