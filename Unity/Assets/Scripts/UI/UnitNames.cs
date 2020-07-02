using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Units;
using UnityEditor.Compilation;
using UnityEngine.UI;
using Utility;

public class UnitNames : MonoBehaviour
{

    
    private TextMeshProUGUI _unitNameText;

    private bool _isTouchingUnit;

    private GameObject _unitNameBox;

    private Image _icon;

    public Sprite _testIcon;
    // Start is called before the first frame update
    void Start()
    {
        _unitNameText = GameSingleton.Instance.uiManager.unitName;
        _unitNameBox = GameSingleton.Instance.uiManager.unitNameBox;
        _icon = GameSingleton.Instance.uiManager.image;
    }
  /*  public void EnterUnit()
    {
        

    }*/

    public void OnMouseEnter()
    {
        Debug.Log("enter unit");
        _unitNameBox.SetActive(true);

        AbstractUnit unit = GetComponent<AbstractUnit>();

        _icon.sprite = GameSingleton.Instance.entityTypeToSprite.GetEntitySprite(unit.GetEntityType());
        Debug.Log("icon is :" + _icon.name);
        _unitNameText.SetText(unit.GetEntityType().ToString());
        //_icon = unit.GetIcon();
        //_icon = _testIcon;

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
