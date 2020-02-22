using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI currencyText;
    private int currency;

    public GameObject InventoryUI;
    
    // Start is called before the first frame update
    void Start()
    {
        currency = 10;
        currencyText.SetText(currency.ToString() + "g");
    }
    
    public void CallButtonInventory()
    {
        InventoryUI.SetActive(!InventoryUI.activeSelf);
    }
}
