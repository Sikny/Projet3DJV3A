using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryContent : MonoBehaviour
{

    public List<ItemSlot> itemSlotsConsumable = new List<ItemSlot>();

    public List<Consummable> inventoryConsummables = new List<Consummable>();
    public List<Equipment> inventoryEquipments = new List<Equipment>();

    private InventoryManager _inventoryManager;
    private ShopManager _shopManager;
    #region Singleton
    
    public static InventoryContent instance;
    
    
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


    private void Start()
    {
        _inventoryManager = InventoryManager.instance;
        _shopManager = ShopManager.instance;
    }

    public void ClearAllItems()
    {
        itemSlotsConsumable.Clear();
        
    }

    public void FillItemSlot(Item newItem)
    {
        int nextIndex = itemSlotsConsumable.Count + 1;
        itemSlotsConsumable[nextIndex].AddItem(newItem);
        

    }

    public void AddItem(Item item)
    {
        _inventoryManager.UpdateGold();
        _shopManager.UpdateGold();
        var itemType = item.GetType();
        
        if (itemType.IsSubclassOf(typeof(Equipment)) || itemType == typeof(Equipment))
        {
            AddEquipment((Equipment)item);
        }
        else if(itemType.IsSubclassOf(typeof(Consummable)) || itemType == typeof(Consummable))
        {
            AddConsummable((Consummable)item);
        }
    }

    public void AddConsummable(Consummable item)
    {
        inventoryConsummables.Add(item);
        _inventoryManager.UpdateUIConsummable(item);
    }

    public void AddEquipment(Equipment equipment)
    {
        inventoryEquipments.Add(equipment);
        _inventoryManager.UpdateUIEquipment(equipment);
    }


    public void RemoveConsummable(Consummable item)
    {
        inventoryConsummables.Remove(item);
        _inventoryManager.RemoveConsummable(item);
    }
    public void AddAllConsummables()
    {
        
        List<Consummable>allConsummables = StoreItems.instance.GetAllItems();

        for(int i = 0; i < allConsummables.Count - 1; i++)
        {
            inventoryConsummables.Add(allConsummables[i]);
        }
    }

    public void AddAllEquipments()
    {
        List<Equipment>allEquipments = StoreItems.instance.GetAllEquipments();

        for(int i = 0; i < allEquipments.Count - 1; i++)
        {
            inventoryEquipments.Add(allEquipments[i]);
        }
    }
    
    public List<Consummable> GetInventoryConsummables()
    {
        return inventoryConsummables;
    }

    public List<Equipment> GetInventoryEquipments()
    {
        return inventoryEquipments;
    }

    
}
