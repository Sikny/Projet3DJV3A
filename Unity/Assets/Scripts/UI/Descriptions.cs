﻿using Items;
using Language;
using TMPro;
using UnityEngine;
using Utility;

namespace UI {
    public class Descriptions : MonoBehaviour
    {
        private TextMeshProUGUI _description;

        private Transform _mousePos;
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
    

        public void EnterItem()
        {
            _descriptionBox.SetActive(true);

            Item item = (isUpgrade) ? upgradeManager.GetUnit(isFirstUpgrade) : GetComponent<ItemSlot>().item;;

            _description.SetText(Traducer.Translate(item.description));
            _isTouchingItem = true;
        }

        public void ExitItem()
        {
            _descriptionBox.SetActive(false);
            _isTouchingItem = false;
        }



        private void Update()
        {
            
            if (_isTouchingItem)
            {
                
                if(Input.mousePosition.x < Screen.width/2f)
                    _descriptionBox.transform.position = new Vector3(Input.mousePosition.x + ( Screen.width / 6f ), Input.mousePosition.y, _descriptionBox.transform.position.z);
                else
                    _descriptionBox.transform.position = new Vector3(Input.mousePosition.x + ( -Screen.width / 6f ), Input.mousePosition.y, _descriptionBox.transform.position.z);

                
                //_descriptionBox.transform.position = new Vector3(Input.mousePosition.x  -90f, Input.mousePosition.y, _descriptionBox.transform.position.z);
            }

        }
    


    }
}
