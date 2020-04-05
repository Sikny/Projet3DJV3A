using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<ItemSlot> _consummableSlots = new List<ItemSlot>();
    private List<GameObject> _consummableGameObjects = new List<GameObject>();
    
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
        UpdateGold();

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

    public void UpdateUIItems()
    {
        
        List<Consummable> inventoryConsummables = _inventoryContent.GetInventoryConsummables();

        for (int i = 0; i < inventoryConsummables.Count; i++)
        {
            
            ItemSlot currentSlot = prefabSlot;

            currentSlot.item = inventoryConsummables[i];
            currentSlot.itemName.SetText(inventoryConsummables[i].name);
            currentSlot.icon.sprite = inventoryConsummables[i].icon;

            _consummableSlots.Add(Instantiate(currentSlot, itemsParent, false));
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

        _consummableSlots.Add(Instantiate(currentSlot, itemsParent, false));


    }
    public void UpdateUIEquipment(Equipment equipment)
    {
        ItemSlot currentSlot = prefabSlot;

        currentSlot.item = equipment;
        currentSlot.itemName.SetText(equipment.name);
        currentSlot.icon.sprite = equipment.icon;

        Instantiate(currentSlot, equipmentsParent, false);
    }

    public void RemoveConsummable(Consummable consummable)
    {
        int targetIndex = 0;
        for (int i = _consummableSlots.Count - 1; i > 0; i--)
        {
            if (_consummableSlots[i].item == consummable)
            {
                targetIndex = i;
                break;
            }
        }
        GameObject slotGameobject = _consummableSlots[targetIndex].gameObject;
        Destroy(slotGameobject);
        _consummableSlots.Remove(_consummableSlots[targetIndex]);
    }
    public void UpdateGold()
    {
        goldText.SetText(Player.instance.gold.ToString() + "g");   
    }

    

}
