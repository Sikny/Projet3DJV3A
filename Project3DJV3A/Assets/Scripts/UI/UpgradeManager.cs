using Game;
using Items;
using Language;
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
            return;
        }

        instance = this;
    }

    #endregion
    
    private Inventory _inventory;

    private void Start()
    {
        //_inventory = GameSingleton.Instance.uiManager.inventory;
    }

    public void ToggleUpgradePannel()
    { 
        _inventory = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
            ? GameSingleton.Instance.GetPlayer().storyModeInventory
            : GameSingleton.Instance.GetPlayer().arcadeModeInventory;
        GameSingleton.Instance.uiManager.ToggleUpgradePanel();
    }
    public void UpdateGold() {
        int gold = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
            ? GameSingleton.Instance.GetPlayer().gold
            : GameSingleton.Instance.GetPlayer().arcadeGold; 
        

        playerGold.SetText(gold+ " g");
    }
    private void UpdateCost(int price) {
        cost.SetText("Costs: " + price+"g");
    }

    public void SetUIUnits(StoreUnit storeUnit)
    {
        GameSingleton.Instance.uiManager.inventoryUi.UpdateGold();
        _currentCost = storeUnit.upgradeCost;
        UpdateCost(_currentCost);
        _unit = storeUnit;
        unitName.SetText(Traducer.Translate(storeUnit.name));
        unitImage.sprite = storeUnit.icon;

        unitUpgrade1Name.SetText(Traducer.Translate(storeUnit.upgrades[0].name));
        unitUpgrade1Image.sprite = storeUnit.upgrades[0].icon;
        
        unitUpgrade2Name.SetText(Traducer.Translate(storeUnit.upgrades[1].name));
        unitUpgrade2Image.sprite = storeUnit.upgrades[1].icon;
        
        ToggleUpgradePannel();
        
    }

    public void OnUpgrade(int number)
    {
        
        Player player = GameSingleton.Instance.GetPlayer();
        Player.Gamemode playerGamemode = player.gamemode;
            
        int gold  = player.gamemode == Player.Gamemode.LEVEL
            ? player.gold
            : player.arcadeGold;
        if (gold >= _currentCost)
        {
            if (playerGamemode == Player.Gamemode.LEVEL)
                player.gold -= _currentCost;
            else
                player.arcadeGold -= _currentCost;
            StoreUnit upgradedUnit = (number == 1) ? _unit.upgrades[0] : _unit.upgrades[1];
            Popups.instance.Popup(Traducer.Translate("Upgraded ") + Traducer.Translate(_unit.name) + Traducer.Translate(" to ") + Traducer.Translate(
                                      upgradedUnit.name));
            _inventory.RemoveUnit(_unit);
            _inventory.AddItem(upgradedUnit);
            ToggleUpgradePannel();
            GameSingleton.Instance.uiManager.descriptionBox.SetActive(false);
        }
        else
        {
            Popups.instance.Popup(Traducer.Translate("Not enough gold!"), Color.red);
            GameSingleton.Instance.uiManager.descriptionBox.SetActive(false);
            ToggleUpgradePannel();
        }

        
    }

    public void OnBack()
    {
        ToggleUpgradePannel();
    }
    public Item GetUnit(bool isFirstUpgrade)
    {
        return (isFirstUpgrade) ?  _unit.upgrades[0] : _unit.upgrades[1];
    }
    
}
