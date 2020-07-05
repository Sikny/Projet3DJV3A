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

        private int _playerGold;
        private Inventory _inventory;

        public void Start() {
            _playerGold = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().gold
                : GameSingleton.Instance.GetPlayer().arcadeGold;
            _inventory = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().storyModeInventory
                : GameSingleton.Instance.GetPlayer().arcadeModeInventory;
            _popup = Popups.instance;
        }

        public void BuyItem()
        {
            Player player = GameSingleton.Instance.GetPlayer();
            Player.Gamemode playerGamemode = player.gamemode;
            
            int gold  = player.gamemode == Player.Gamemode.LEVEL
                ? player.gold
                : player.arcadeGold;
            
            if (gold >= item.price) {
                if (playerGamemode == Player.Gamemode.LEVEL)
                    player.gold -= item.price;
                else
                    player.arcadeGold -= item.price;
                ItemSlot boughtSlot = inventoryPrefab;
                boughtSlot.item = item;
                boughtSlot.icon.sprite = icon.sprite;
                boughtSlot.itemName.SetText(itemName.text);
                _inventory.AddItem(boughtSlot.item);
                _popup.Popup("Bought " + item.name + "!");
            }
            else
            {
                _popup.Popup("Not enough gold!", Color.red);
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