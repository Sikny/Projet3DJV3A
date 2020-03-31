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

    public List<Item> allItems;
    
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
        
        if(itemSlotsConsumable[nextIndex].onItemChangedCallback != null)
            itemSlotsConsumable[nextIndex].onItemChangedCallback.Invoke();
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
