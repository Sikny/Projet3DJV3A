using System.Collections;
using System.Collections.Generic;
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
    public GameObject ShopPanel;

    public GameObject prefab; 
    
    public delegate void onUpdateUI(); 
    public onUpdateUI onUpdateUICallback;

    private ShopContent _shopContent;
    
    
    //-----------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        _shopContent = ShopContent.instance;
        gold = 10;
        goldText.SetText(gold.ToString() + "g");
        
        /*GameObject currentSlot = prefab;
        TextMeshProUGUI[] test = currentSlot.GetComponentsInChildren<TextMeshProUGUI>();
        test[0].SetText("item name");
        test[1].SetText("price");*/
        _shopContent.AddAllItems();
        _shopContent.AddAllEquipments();
        UpdateUIItems();
        UpdateUIEquipments();
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

    public void UpdateUIItems()
    {
        List<Item> shopItems = _shopContent.GetShopItems();

        for (int i = 0; i < shopItems.Count - 1; i++)
        {
            
            GameObject currentSlot = prefab;
            currentSlot.transform.SetParent(itemsPanel.transform);
            TextMeshProUGUI[] texts = currentSlot.GetComponentsInChildren<TextMeshProUGUI>();
            Image icon = currentSlot.GetComponentInChildren<Image>(); 
            
            texts[0].SetText(shopItems[i].name);
            texts[1].SetText(shopItems[i].price.ToString());
            icon.sprite = shopItems[i].icon;
            
            Instantiate(currentSlot);
        }
    }
    
    public void UpdateUIEquipments()
    {
        List<Equipment> shopEquipments = _shopContent.GetShopEquipments();

        for (int i = 0; i < shopEquipments.Count - 1; i++)
        {
            
            GameObject currentSlot = prefab;
            currentSlot.transform.SetParent(equipmentsPanel.transform);
            TextMeshProUGUI[] texts = currentSlot.GetComponentsInChildren<TextMeshProUGUI>();
            Image icon = currentSlot.GetComponentInChildren<Image>(); 
            
            texts[0].SetText(shopEquipments[i].name);
            texts[1].SetText(shopEquipments[i].price.ToString());
            icon.sprite = shopEquipments[i].icon;
            
            Instantiate(currentSlot);
        }
    }
    
    public void Fight()
    {
        ShopPanel.SetActive(false);
    }
    

    

}
