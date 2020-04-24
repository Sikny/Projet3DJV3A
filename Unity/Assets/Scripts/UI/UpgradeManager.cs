﻿using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Items;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class UpgradeManager : MonoBehaviour
{

    public GameObject upgradePanel; 
    
    public Image unitImage;
    public TextMeshProUGUI unitName;
    //public TextMeshProUGUI Description etc
    [Space]
    public Image unitUpgrade1Image;
    public TextMeshProUGUI unitUpgrade1Name;
    [Space]
    public Image unitUpgrade2Image;
    public TextMeshProUGUI unitUpgrade2Name;
    
    private StoreUnit _unit;
    
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
    
    private Inventory _inventory;

    private void Start()
    {

        _inventory = GameSingleton.Instance.uiManager.inventory;
    }

    public void ToggleUpgradePannel()
    {
        upgradePanel.SetActive(!upgradePanel.activeSelf);
    }

    public void SetUIUnits(StoreUnit storeUnit)
    {
        _unit = storeUnit;
        Debug.Log("" + storeUnit.upgrades[0].entityType + storeUnit.upgrades[1].entityType);
        unitName.SetText(storeUnit.entityType.ToString());
        unitImage.sprite = storeUnit.icon;

        unitUpgrade1Name.SetText(storeUnit.upgrades[0].entityType.ToString());
        unitUpgrade1Image.sprite = storeUnit.upgrades[0].icon;
        
        unitUpgrade2Name.SetText(storeUnit.upgrades[1].entityType.ToString());
        unitUpgrade2Image.sprite = storeUnit.upgrades[1].icon;
        
        ToggleUpgradePannel();

        //Entity unit;
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

    public void OnUpgrade(int number)
    {        
        StoreUnit upgradedUnit = (number == 1) ? _unit.upgrades[0] : _unit.upgrades[1];
        _inventory.RemoveUnit(_unit);
        _inventory.AddItem(upgradedUnit);
        ToggleUpgradePannel();
        
    }
    
}
