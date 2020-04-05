using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Consummable")]
public class Consummable : Item
{
    //public String description = ""; 

    public override void Use()
    {
        base.Use();
        RemoveFromInventory();
    }
    
    public void RemoveFromInventory ()
    {
        InventoryContent.instance.RemoveConsummable(this);
    }
}