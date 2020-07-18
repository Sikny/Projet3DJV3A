using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Items;
using Language;
using TMPro;
using Units;
using UnityEngine;
using Utility;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;


namespace UI {
    public class InventoryManager : ItemsPanel {
        public TextMeshProUGUI goldText;

        public ItemSlot prefabSlot;

        public ItemSlot unitPrefabSlot;

        private List<ItemSlot> _consumableSlots = new List<ItemSlot>();

        private List<ItemSlot> _unitSlots = new List<ItemSlot>();

        void Start() {
            //inventoryPanel.SetActive(false);
            Player player = GameSingleton.Instance.GetPlayer();
            if (player.currentLevel == 0 && !player.storyModeInventory.units.Any() && player.gamemode == Player.Gamemode.LEVEL) 
            {
                for (int i = 0; i < 2; i++)
                {
                    int seed = player.currentSeed;
                    //int seed = UnityRandom.Range(Int32.MinValue, Int32.MaxValue);
                    Random rand = new Random(DateTime.Now.Millisecond);


                    int randEntity = rand.Next(0, 3);
                    EntityType entityType = EntityType.Soldier;
                    switch (randEntity)
                    {
                        case 0:
                            entityType = EntityType.Soldier;
                            break;
                        case 1:
                            entityType = EntityType.Archer;
                            break;
                        case 2:
                            entityType = EntityType.Mage;
                            break;
                    }
                    StoreUnit storeUnit = GameSingleton.Instance.storeUnitList.GetStoreUnitByEntityType(entityType);
                    player.storyModeInventory.AddItem(storeUnit);
                }
            }
                   
            Debug.Log("setting start gold to : " + player.gold);
            Debug.Log("setting inventory to : ");
            foreach (var unit in player.storyModeInventory.units)
            {
                Debug.Log(unit.itemName);
            }
            player.goldStartLevel = player.gold;
            player.inventoryStartLevel = player.storyModeInventory; 
            UpdateGold();
        }

        public void UpdateUiConsumable(Consumable consumable) {
            var addedItem = Instantiate(prefabSlot, itemsParent, false);
            addedItem.item = consumable;
            addedItem.itemName.SetText(Traducer.Translate(consumable.itemName));
            addedItem.icon.sprite = consumable.icon;

            _consumableSlots.Add(addedItem);
        }

        public void UpdateUiEquipment(Equipment equipment) {
            var addedItem = Instantiate(prefabSlot, equipmentsParent, false);
            addedItem.item = equipment;
            addedItem.itemName.SetText(Traducer.Translate(equipment.itemName));
            addedItem.icon.sprite = equipment.icon;
        }

        public void UpdateUiUnit(StoreUnit unit)
        {
            ItemSlot targetSlot = (unit.upgrades.Count > 0) ? unitPrefabSlot : prefabSlot;
 
            var addedItem = Instantiate(targetSlot, unitsParent, false);
            
            addedItem.item = unit;
            addedItem.itemName.SetText(Traducer.Translate(unit.itemName));
            addedItem.icon.sprite = unit.icon;
            
            _unitSlots.Add(addedItem);
        }

        public void RemoveConsumable(Consumable consumable) {
            int targetIndex = 0;
            for (int i = _consumableSlots.Count - 1; i > 0; i--) {
                if (_consumableSlots[i].item == consumable) {
                    targetIndex = i;
                    break;
                }
            }

            GameObject slotGameObject = _consumableSlots[targetIndex].gameObject;
            Destroy(slotGameObject);
            _consumableSlots.Remove(_consumableSlots[targetIndex]);
        }

        public void RemoveUnit(StoreUnit unit) {
            int targetIndex = 0;
            for (int i = _unitSlots.Count - 1; i > 0; i--) {
                if (_unitSlots[i].item == unit) {
                    targetIndex = i;
                    break;
                }
            }

            GameObject slotGameObject = _unitSlots[targetIndex].gameObject;
            Destroy(slotGameObject);
            _unitSlots.Remove(_unitSlots[targetIndex]);
        }

        public void ClearUiInventory()
        {
            _unitSlots.Clear();
            _consumableSlots.Clear();
        }
  

        public void UpdateGold() {
            int gold = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().gold
                : GameSingleton.Instance.GetPlayer().arcadeGold;
            goldText.SetText(gold + " g");
        }
    }
}