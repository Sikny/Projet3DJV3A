using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    public delegate void onItemChanged();
    public onItemChanged onItemChangedCallback;

    public GameObject prefab;
    
    public Image icon;


    public Item item;

    private ShopManager _shopManager;

    public void BuyItem()
    {
        if (Player.instance.gold > item.price)
        {
            Player.instance.gold -= item.price;
            //Player.instance.inventory.add(item);
        }
    }


    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }
    public void RemoveItem()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
