using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
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
        
    public Transform itemsParent;
    public Transform equipmentsParent;
    public Transform unitsParent;
    
    //-----------------------------------------------------------------------------

    private List<ItemSlot> _consummableSlots = new List<ItemSlot>();
    private List<GameObject> _consummableGameObjects = new List<GameObject>();
    
    private List<ItemSlot> _unitSlots = new List<ItemSlot>();

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

    /*public void UpdateUIItems()
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
    }*/

    public void UpdateUIConsummable(Consumable consumable)
    {
        ItemSlot currentSlot = prefabSlot;

        currentSlot.item = consumable;
        currentSlot.itemName.SetText(consumable.itemName);
        currentSlot.icon.sprite = consumable.icon;

        _consummableSlots.Add(Instantiate(currentSlot, itemsParent, false));


    }
    public void UpdateUIEquipment(Equipment equipment)
    {
        ItemSlot currentSlot = prefabSlot;

        currentSlot.item = equipment;
        currentSlot.itemName.SetText(equipment.itemName);
        currentSlot.icon.sprite = equipment.icon;

        Instantiate(currentSlot, equipmentsParent, false);
    }

    public void UpdateUIUnit(StoreUnit unit) {
        ItemSlot currentSlot = prefabSlot;

        currentSlot.item = unit;
        currentSlot.itemName.SetText(unit.itemName);
        currentSlot.icon.sprite = unit.icon;

        _unitSlots.Add(Instantiate(currentSlot, unitsParent, false));
    }

    public void RemoveConsummable(Consumable consumable)
    {
        int targetIndex = 0;
        for (int i = _consummableSlots.Count - 1; i > 0; i--)
        {
            if (_consummableSlots[i].item == consumable)
            {
                targetIndex = i;
                break;
            }
        }
        GameObject slotGameobject = _consummableSlots[targetIndex].gameObject;
        Destroy(slotGameobject);
        _consummableSlots.Remove(_consummableSlots[targetIndex]);
    }

    public void RemoveUnit(StoreUnit unit) {
        int targetIndex = 0;
        for (int i = _unitSlots.Count - 1; i > 0; i--) {
            if (_unitSlots[i].item == unit) {
                targetIndex = i;
                break;
            }
        }

        GameObject slotGameObject = _unitSlots[targetIndex].gameObject;
        Destroy(slotGameObject);
        _unitSlots.Remove(_unitSlots[targetIndex]);
    }
    
    public void UpdateGold()
    {
        goldText.SetText(Player.instance.gold.ToString() + "g");   
    }

    

}
