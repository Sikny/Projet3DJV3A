using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    public void EnterUnit()
    {
        Debug.Log("enter");
        _unitNameBox.SetActive(true);

        
        //Item item = (isUpgrade) ? upgradeManager.GetUnit(isFirstUpgrade) : GetComponent<ItemSlot>().item;;

        //_unitNameText.SetText(item.description);
        _isTouchingUnit = true;
    }

    public void ExitUnit()
    {
        Debug.Log("exit");

        _unitNameBox.SetActive(false);
        _isTouchingUnit = false;
    }



    private void Update()
    {
        if (_isTouchingUnit)
        {
            _unitNameBox.transform.position = new Vector3(Input.mousePosition.x + ( -Screen.width / 8f ), Input.mousePosition.y, _unitNameBox.transform.position.z);
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