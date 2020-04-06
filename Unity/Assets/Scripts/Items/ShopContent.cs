using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class ShopContent
{
    /*public List<Item> itemsConsummable = new List<Item>();
    public List<Item> itemsEquipment = new List<Item>();
    public List<Item> itemsUnits = new List<Item>();*/

    private List<ItemSlot> itemSlotsConsumable = new List<ItemSlot>();
    
    public List<Consummable> shopConsummables = new List<Consummable>();
    public List<Equipment> shopEquipments = new List<Equipment>();
    public List<StoreUnit> shopUnits = new List<StoreUnit>();

    #region Singleton
    
    private static ShopContent _instance;

    public static ShopContent Instance {
        get {
            if(_instance == null) _instance = new ShopContent();
            return _instance;
        }
    }

    #endregion
    
    public void ClearAllItems()
    {
        itemSlotsConsumable.Clear();
    }

    public void FillItemSlot(Item newItem)
    {
        int nextIndex = itemSlotsConsumable.Count + 1;
        itemSlotsConsumable[nextIndex].AddItem(newItem);
    }

    public void AddConsummable(Consummable item)
    {
        shopConsummables.Add(item);
    }

    public void AddEquipment(Equipment equipment)
    {
        shopEquipments.Add(equipment);
    }

    public void AddStoreUnit(StoreUnit unit) {
        shopUnits.Add(unit);
    }

    public void ClearShop() {
        shopConsummables.Clear();
        shopEquipments.Clear();
        shopUnits.Clear();
    }
}
