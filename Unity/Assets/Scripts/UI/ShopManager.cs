using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Object = System.Object;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    private int gold = 5;

    public GameObject unitPanel;
    public GameObject itemsPanel;
    public GameObject equipmentsPanel;
    public GameObject shopPanel;

    public ItemSlot prefabSlot; 
        
    private Shop _shop;
    public Transform itemsParent;
    public Transform equipmentsParent;
    public Transform unitsParent;

    private Player _player;

    //-----------------------------------------------------------------------------

    #region Singleton
    
    public static ShopManager instance;
    
    
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
    // Start is called before the first frame update
    void Start()
    {
        
        _shop = Shop.Instance;
        _player = Player.instance;
        gold = 10;
        goldText.SetText(gold + "g");
        
        UpdateUIItems();
        UpdateUIEquipments();
        UpdateUIUnits();
        UpdateGold();
        shopPanel.SetActive(false);
    }



    public void ShowUnitsPanel()
    {
        itemsPanel.SetActive(false);
        equipmentsPanel.SetActive(false);
        unitPanel.SetActive(true);
    }
    
    public void ShowItemsPanel()
    {
        equipmentsPanel.SetActive(false);
        unitPanel.SetActive(false);
        itemsPanel.SetActive(true);
    }

    public void ShowEquipmentsPanel()
    {
        itemsPanel.SetActive(false);
        unitPanel.SetActive(false);
        equipmentsPanel.SetActive(true);
    }
    
    public void ActivateSelf()
    {
        GameObject o = gameObject;
        UIManager.clearUI(o.transform.parent.gameObject,1);
        o.SetActive(!o.activeSelf);
        systemUnit.SetRunning(!o.activeSelf);
    }


    private void UpdateUIItems()
    {
        List<Consumable> shopConsummables = _shop.shopConsummables;
        //List<Item> shopConsummables = _shopContent.GetShopItems().Cast<Item>().ToList();

        //Debug.Log("shop items count: " + shopConsummables.Count);
        for (int i = 0; i < shopConsummables.Count; i++)
        {
            
            ItemSlot currentSlot = prefabSlot;

            currentSlot.item = shopConsummables[i];
            currentSlot.itemName.SetText(shopConsummables[i].itemName);
            currentSlot.icon.sprite = shopConsummables[i].icon;
            currentSlot.price.SetText(shopConsummables[i].price.ToString());
            //Debug.Log("put item :" + shopConsummables[i].name);

            Instantiate(currentSlot, itemsParent, false);
            //Canvas.ForceUpdateCanvases();
        }
    }
    
    private void UpdateUIEquipments()
    {
        List<Equipment> shopEquipments = _shop.shopEquipments;
        //Debug.Log("shop EQUIPMENTS count: " + shopEquipments.Count);

        for (int i = 0; i < shopEquipments.Count; i++)
        {
            ItemSlot currentSlot = prefabSlot;
            currentSlot.item = shopEquipments[i];
            currentSlot.itemName.SetText(shopEquipments[i].itemName);
            currentSlot.icon.sprite = shopEquipments[i].icon;

            //Debug.Log("put equipment :" + shopEquipments[i].name);
            
            Instantiate(currentSlot, equipmentsParent, false);
            //Canvas.ForceUpdateCanvases();
           

        }
    }

    private void UpdateUIUnits() {
        List<StoreUnit> shopUnits = _shop.shopUnits;

        for (int i = 0; i < shopUnits.Count; i++) {
            ItemSlot currentSlot = prefabSlot;
            currentSlot.item = shopUnits[i];
            currentSlot.itemName.SetText(shopUnits[i].itemName);
            currentSlot.icon.sprite = shopUnits[i].icon;

            Instantiate(currentSlot, unitsParent, false);
        }
    }

    public void UpdateGold()
    {
        goldText.SetText(_player.GetGold() + "g");   
    }

    public GameObject fightButton;
    public SystemUnit systemUnit;
    
    public void Fight() {
        int playerCount = FindObjectsOfType<PlayerUnit>().Length;
        if (playerCount == 0) return;
        systemUnit.SetRunning(true);
        fightButton.SetActive(false);
        shopPanel.SetActive(false);
        GameSingleton.Instance.StartFight();
    }
    

    

}
