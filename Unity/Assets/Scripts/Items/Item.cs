using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon = null;
    public int price = 10;
    //public String description = ""; 
    
    // Called when the item is pressed in the inventory
    public virtual void Use ()
    {
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory ()
    {
        //TODO
    }
    
}
