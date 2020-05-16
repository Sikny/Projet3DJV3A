using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Packages.Rider.Editor.UnitTesting;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

public class Descriptions : MonoBehaviour
{
    private TextMeshProUGUI _description;

    private bool isTouchingItem = false;

    private GameObject _descriptionBox;


    private void Start()
    {
        _descriptionBox = GameSingleton.Instance.uiManager.descriptionBox;
        _description = GameSingleton.Instance.uiManager.description;
    }
    

    public void EnterItem()
    {
        _descriptionBox.SetActive(true);
        Item item = GetComponent<ItemSlot>().item;
        _description.SetText(item.description);
        isTouchingItem = true;
    }

    public void ExitItem()
    {
        _descriptionBox.SetActive(false);
        isTouchingItem = false;
    }



    private void Update()
    {
        if (isTouchingItem)
        {
            _descriptionBox.transform.position = new Vector3(Input.mousePosition.x-70, Input.mousePosition.y+20, _descriptionBox.transform.position.z);
        }

    }
    


}
