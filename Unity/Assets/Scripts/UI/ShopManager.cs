using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    private int gold = 5;

    public GameObject unitPanel;
    public GameObject itemsPanel;
    public GameObject equipmentsPanel;
    public GameObject shopPanel;

    public ItemSlot prefabSlot; 
        
    private ShopContent _shopContent;
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
        
        _shopContent = ShopContent.Instance;
        _player = Player.instance;
        gold = 10;
        goldText.SetText(gold + "g");
        

        _shopContent.AddAllConsummables();
        _shopContent.AddAllEquipments();
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
        gameObject.SetActive(!gameObject.activeSelf);
    }


    private void UpdateUIItems()
    {
        List<Consummable> shopConsummables = _shopContent.shopConsummables;
        //List<Item> shopConsummables = _shopContent.GetShopItems().Cast<Item>().ToList();

        //Debug.Log("shop items count: " + shopConsummables.Count);
        for (int i = 0; i < shopConsummables.Count; i++)
        {
            
            ItemSlot currentSlot = prefabSlot;

            currentSlot.item = shopConsummables[i];
            currentSlot.itemName.SetText(shopConsummables[i].name);
            currentSlot.icon.sprite = shopConsummables[i].icon;
            currentSlot.price.SetText(shopConsummables[i].price.ToString());
            //Debug.Log("put item :" + shopConsummables[i].name);

            Instantiate(currentSlot, itemsParent, false);
            //Canvas.ForceUpdateCanvases();
        }
    }
    
    private void UpdateUIEquipments()
    {
        List<Equipment> shopEquipments = _shopContent.shopEquipments;
        //Debug.Log("shop EQUIPMENTS count: " + shopEquipments.Count);

        for (int i = 0; i < shopEquipments.Count; i++)
        {
            ItemSlot currentSlot = prefabSlot;
            currentSlot.item = shopEquipments[i];
            string itname = shopEquipments[i].name;
            currentSlot.itemName.SetText(itname);
            currentSlot.icon.sprite = shopEquipments[i].icon;

            //Debug.Log("put equipment :" + shopEquipments[i].name);
            
            Instantiate(currentSlot, equipmentsParent, false);
            //Canvas.ForceUpdateCanvases();
           

        }
    }

    private void UpdateUIUnits() {
        List<StoreUnit> shopUnits = _shopContent.shopUnits;

        for (int i = 0; i < shopUnits.Count; i++) {
            ItemSlot currentSlot = prefabSlot;
            currentSlot.item = shopUnits[i];
            currentSlot.itemName.SetText(shopUnits[i].name);
            currentSlot.icon.sprite = shopUnits[i].icon;

            Instantiate(currentSlot, unitsParent, false);
        }
    }

    public void UpdateGold()
    {
        goldText.SetText(_player.GetGold() + "g");   
    }

    public void Fight()
    {
        shopPanel.SetActive(false);
    }
    

    

}
