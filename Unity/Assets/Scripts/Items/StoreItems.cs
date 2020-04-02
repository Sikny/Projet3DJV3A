using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItems : MonoBehaviour
{
    public List<Consummable> allConssumables;
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
    public List<Consummable> GetAllItems()
    {
        return allConssumables;
    }

    public List<Equipment> GetAllEquipments()
    {
        return allEquipments;
    }


    
}
