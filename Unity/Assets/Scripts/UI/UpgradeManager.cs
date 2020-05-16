using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Items;
using TMPro;
using UI;
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

    public TextMeshProUGUI playerGold;
    public TextMeshProUGUI cost;

    private int _currentCost;
    
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
    private void UpdateGold() {
        playerGold.SetText(GameSingleton.Instance.GetPlayer().GetGold() + "g");
    }
    private void UpdateCost(int price) {
        cost.SetText("Costs: " + price+"g");
    }

    public void SetUIUnits(StoreUnit storeUnit)
    {
        
        UpdateGold();
        _currentCost = storeUnit.upgradeCost;
        UpdateCost(_currentCost);
        _unit = storeUnit;
        unitName.SetText(storeUnit.name);
        unitImage.sprite = storeUnit.icon;

        unitUpgrade1Name.SetText(storeUnit.upgrades[0].name);
        unitUpgrade1Image.sprite = storeUnit.upgrades[0].icon;
        
        unitUpgrade2Name.SetText(storeUnit.upgrades[1].name);
        unitUpgrade2Image.sprite = storeUnit.upgrades[1].icon;
        
        ToggleUpgradePannel();
        
    }

    public void OnUpgrade(int number)
    {
        if (GameSingleton.Instance.GetPlayer().GetGold() >= _currentCost)
        {
            GameSingleton.Instance.GetPlayer().gold -= _currentCost;
            StoreUnit upgradedUnit = (number == 1) ? _unit.upgrades[0] : _unit.upgrades[1];
            Popups.instance.Popup("Upgraded " + _unit.name + " to " + upgradedUnit.name);
            _inventory.RemoveUnit(_unit);
            _inventory.AddItem(upgradedUnit);
            ToggleUpgradePannel();
        }
        else
        {
            Popups.instance.Popup("Not enough gold!");
            ToggleUpgradePannel();
        }

        
    }

    public Item GetUnit(bool isFirstUpgrade)
    {
        return (isFirstUpgrade) ?  _unit.upgrades[0] : _unit.upgrades[1];
    }
    
}
