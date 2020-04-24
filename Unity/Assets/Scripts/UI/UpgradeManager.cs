using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public Image unitImage;
    public TextMeshProUGUI unitName;
    //public TextMeshProUGUI Description etc
    [Space]
    public Image unitUpgrade1Image;
    public TextMeshProUGUI unitUpgrade1Name;
    [Space]
    public Image unitUpgrade2Image;
    public TextMeshProUGUI unitUpgrade2Name;

    public EntityDict entityDict;

    #region Singleton

    public static UpgradeManager instance;


    private void Awake() {
        if (instance != null) {
            Debug.Log("Several instances");
            return;
        }

        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUIUnits(StoreUnit storeUnit)
    {
        Debug.Log("" + storeUnit.upgrades[0].entityType + storeUnit.upgrades[1].entityType);
        unitName.SetText(storeUnit.entityType.ToString());
        unitImage.sprite = storeUnit.icon;

        unitUpgrade1Name.SetText(storeUnit.upgrades[0].entityType.ToString());
        unitUpgrade1Image.sprite = storeUnit.upgrades[0].icon;
        
        unitUpgrade2Name.SetText(storeUnit.upgrades[1].entityType.ToString());
        unitUpgrade2Image.sprite = storeUnit.upgrades[1].icon;
        
        Entity unit;
        //unit = entityDict.GetEntityType(unitID);
        /*List<EntityType> upgrades = unit.upgrades;
        for(int i = 0; i < upgrades.Count - 1; i++)
        {
            Debug.Log("Entity : "  + upgrades[i]);
        }
        //entry unit from inventory
        /*
         * get model image
         * get name
         * get upgrades and do the same for them
         * 
         */
    }
}
