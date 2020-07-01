using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Units;
using UnityEngine.UI;
using Utility;

public class UnitNames : MonoBehaviour
{

    private TextMeshProUGUI _unitNameText;

    private bool _isTouchingUnit;

    private GameObject _unitNameBox;
    
    // Start is called before the first frame update
    void Start()
    {
        _unitNameText = GameSingleton.Instance.uiManager.unitName;
        _unitNameBox = GameSingleton.Instance.uiManager.unitNameBox;
    }
  /*  public void EnterUnit()
    {
        

    }*/

    public void OnMouseEnter()
    {
        Debug.Log("enter unit");
        _unitNameBox.SetActive(true);

        AbstractUnit unit = GetComponent<AbstractUnit>();

        //Item item = (isUpgrade) ? upgradeManager.GetUnit(isFirstUpgrade) : GetComponent<ItemSlot>().item;;
        _unitNameText.SetText(unit.GetEntityType().ToString());
        //_unitNameText.SetText(item.description);
        _isTouchingUnit = true;
    }

    public void OnMouseExit()
    {
        Debug.Log("exit unit");

        _unitNameBox.SetActive(false);
        _isTouchingUnit = false;
    }
/*
    public void ExitUnit()
    {

    }*/



    private void Update()
    {
        if (_isTouchingUnit)
        {
            _unitNameBox.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y+  ( Screen.height / 8f ), _unitNameBox.transform.position.z);
        }

    }


}
/*
public class Descriptions : MonoBehaviour
{
    private TextMeshProUGUI _description;

    private bool _isTouchingItem;

    private GameObject _descriptionBox;

    public bool isUpgrade;
    public bool isFirstUpgrade;

    public UpgradeManager upgradeManager;

    private void Start()
    {
        _descriptionBox = GameSingleton.Instance.uiManager.descriptionBox;
        _description = GameSingleton.Instance.uiManager.description;
    }
    




}
*/