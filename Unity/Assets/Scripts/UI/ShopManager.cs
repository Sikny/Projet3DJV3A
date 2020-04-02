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
    public ItemSlot prefabSlot; 
        
    public delegate void onUpdateUI(); 
    public onUpdateUI onUpdateUICallback;

    private ShopContent _shopContent;
    public Transform itemsParent;
    public Transform equipmentsParent;
    public Transform unitsParent;

    public Image slotBackground;
    
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
        //Debug.Log("shop items count: " + shopItems.Count);
        for (int i = 0; i < shopItems.Count; i++)
        {
            
            ItemSlot currentSlot = prefabSlot;

            currentSlot.itemName.SetText(shopItems[i].name);
            currentSlot.icon.sprite = shopItems[i].icon;
            currentSlot.price.SetText(shopItems[i].price.ToString());
            //Debug.Log("put item :" + shopItems[i].name);

            Instantiate(currentSlot, itemsParent, false);
            //Canvas.ForceUpdateCanvases();
        }
    }
    
    public void UpdateUIEquipments()
    {
        List<Equipment> shopEquipments = _shopContent.GetShopEquipments();
        //Debug.Log("shop EQUIPMENTS count: " + shopEquipments.Count);

        for (int i = 0; i < shopEquipments.Count; i++)
        {

            ItemSlot currentSlot = prefabSlot;
            
            currentSlot.itemName.SetText(shopEquipments[i].name);
            currentSlot.icon.sprite = shopEquipments[i].icon;
            currentSlot.price.SetText(shopEquipments[i].price.ToString());
            //Debug.Log("put equipment :" + shopEquipments[i].name);

            Instantiate(currentSlot, equipmentsParent, false);
            //Canvas.ForceUpdateCanvases();
           

        }
    }
    
    public void Fight()
    {
        ShopPanel.SetActive(false);
    }
    

    

}
