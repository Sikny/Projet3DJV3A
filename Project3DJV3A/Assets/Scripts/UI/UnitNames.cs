using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class UnitNames : MonoBehaviour
    {
        private TextMeshProUGUI _unitNameText;

        private bool _isTouchingUnit;

        private GameObject _unitNameBox;

        private Image _icon;
    
        private readonly Color _red = Color.red;
        private readonly Color _green = Color.green;
        private UiManager _uiManager;

        private bool _uiActivated;

        // Start is called before the first frame update
        void Start()
        {
            _unitNameText = GameSingleton.Instance.uiManager.unitName;
            _unitNameBox = GameSingleton.Instance.uiManager.unitNameBox;
            _icon = GameSingleton.Instance.uiManager.image;
        }

        public void OnMouseEnter()
        {
            if (_uiActivated) return;
            _unitNameBox.SetActive(true);

            AbstractUnit unit = GetComponent<AbstractUnit>();

            _icon.sprite = GameSingleton.Instance.entityTypeToSprite.GetEntitySprite(unit.GetEntityType());
        
            var unitType = unit.GetType();
            _unitNameText.color = unitType == typeof(AiUnit) ? _red : _green;
        
            _unitNameText.SetText(unit.GetEntityType().ToString());
            _isTouchingUnit = true;
        }

        public void OnMouseExit()
        {
            _unitNameBox.SetActive(false);
            _isTouchingUnit = false;
        }

        private void Update()
        {
            if (!_uiManager && GameSingleton.Instance && GameSingleton.Instance.uiManager) {
                _uiManager = GameSingleton.Instance.uiManager;
            }
            else if (_uiManager)
            {
                if (_uiManager.upgradePanel.activeSelf || _uiManager.pausePanel.activeSelf ||
                    GameSingleton.Instance.endGamePanel.winMessage.IsActive() ||
                    GameSingleton.Instance.endGamePanel.loseMessage.IsActive()) _uiActivated = true;
                else
                    _uiActivated = false;
            }
            if (_isTouchingUnit && !_uiActivated)
            {
                _unitNameBox.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y+  ( Screen.height / 8f ), _unitNameBox.transform.position.z);
            }
        }
    }
}
