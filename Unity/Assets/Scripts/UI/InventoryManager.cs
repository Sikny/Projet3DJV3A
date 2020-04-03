using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{    
    public TextMeshProUGUI goldText;


    public GameObject unitPanel;
    public GameObject itemsPanel;
    public GameObject equipmentsPanel;
    public GameObject inventoryPanel;

    public ItemSlot prefabSlot; 
        
    private InventoryContent _inventoryContent;
    public Transform itemsParent;
    public Transform equipmentsParent;
    public Transform unitsParent;
    
    //-----------------------------------------------------------------------------

    #region Singleton
    
    public static InventoryManager instance;
    
    
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
        _inventoryContent = InventoryContent.instance;
        
        inventoryPanel.SetActive(false);

        /*_inventoryContent.AddAllConsummables();
        _inventoryContent.AddAllEquipments();
        UpdateUIItems();
        UpdateUIEquipments();*/
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
        
        List<Consummable> inventoryConsummables = _inventoryContent.GetInventoryConsummables();

        for (int i = 0; i < inventoryConsummables.Count; i++)
        {
            
            ItemSlot currentSlot = prefabSlot;

            currentSlot.item = inventoryConsummables[i];
            currentSlot.itemName.SetText(inventoryConsummables[i].name);
            currentSlot.icon.sprite = inventoryConsummables[i].icon;

            Instantiate(currentSlot, itemsParent, false);
        }
    }


    
    public void UpdateUIEquipments()
    {
        List<Equipment> inventoryEquipments = _inventoryContent.GetInventoryEquipments();

        for (int i = 0; i < inventoryEquipments.Count; i++)
        {

            ItemSlot currentSlot = prefabSlot;
            currentSlot.item = inventoryEquipments[i];
            currentSlot.itemName.SetText(inventoryEquipments[i].name);
            currentSlot.icon.sprite = inventoryEquipments[i].icon;

            
            Instantiate(currentSlot, equipmentsParent, false);
           
        }
    }

    public void UpdateUIConsummable(Consummable consummable)
    {
        ItemSlot currentSlot = prefabSlot;

        currentSlot.item = consummable;
        currentSlot.itemName.SetText(consummable.name);
        currentSlot.icon.sprite = consummable.icon;

        Instantiate(currentSlot, itemsParent, false);
    }
    public void UpdateUIEquipment(Equipment equipment)
    {
        ItemSlot currentSlot = prefabSlot;

        currentSlot.item = equipment;
        currentSlot.itemName.SetText(equipment.name);
        currentSlot.icon.sprite = equipment.icon;

        Instantiate(currentSlot, equipmentsParent, false);
    }
    public void UpdateGold()
    {
        goldText.SetText(Player.instance.gold.ToString() + "g");   
    }

    

}
