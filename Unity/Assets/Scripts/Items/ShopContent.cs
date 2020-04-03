using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopContent : MonoBehaviour
{
    /*public List<Item> itemsConsummable = new List<Item>();
    public List<Item> itemsEquipment = new List<Item>();
    public List<Item> itemsUnits = new List<Item>();*/

    public List<ItemSlot> itemSlotsConsumable = new List<ItemSlot>();


    public List<Consummable> shopConsummables = new List<Consummable>();
    public List<Equipment> shopEquipments = new List<Equipment>();

    
    
    #region Singleton
    
    public static ShopContent instance;
    
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Several instances");
            return;
        }
        instance = this;
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

    public void AddAllConsummables()
    {
        
        List<Consummable>allConsummables = StoreItems.instance.GetAllItems();

        for(int i = 0; i < allConsummables.Count - 1; i++)
        {
            shopConsummables.Add(allConsummables[i]);
        }
    }

    public void AddAllEquipments()
    {
        List<Equipment>allEquipments = StoreItems.instance.GetAllEquipments();

        for(int i = 0; i < allEquipments.Count - 1; i++)
        {
            shopEquipments.Add(allEquipments[i]);
        }
    }
    
    public List<Consummable> GetShopConsummables()
    {
        return shopConsummables;
    }

    public List<Equipment> GetShopEquipments()
    {
        return shopEquipments;
    }

/*
    public void AddConsummable(Item item)
    {
        itemsConsummable.Add(item);
    }

    public void RemoveConsummable(Item item)
    {
        itemsConsummable.Remove(item);
    }
    
    public void AddEquipment(Item item)
    {
        itemsEquipment.Add(item);
    }

    public void RemoveEquipment(Item item)
    {
        itemsEquipment.Remove(item);
    }
    public void AddUnit(Item item)
    {
        itemsUnits.Add(item);
    }

    public void RemoveUnit(Item item)
    {
        itemsUnits.Remove(item);
    }*/
    
}
