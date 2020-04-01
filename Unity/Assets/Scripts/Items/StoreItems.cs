using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItems : MonoBehaviour
{
    public List<Item> allItems;
    public List<Equipment> allEquipments;

    
    #region Singleton
    
    public static StoreItems instance;
    
    
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
    public List<Item> GetAllItems()
    {
        return allItems;
    }

    public List<Equipment> GetAllEquipments()
    {
        return allEquipments;
    }

    public void helloworld()
    { 
        Debug.Log("hey there");
    }
    
}
