using UI;
using UnityEngine;
using Utility;

namespace Terrain {
    public class TerrainRaycaster : MonoBehaviour {
        private Camera _cam;
        public int height;
        public int width;
        private UiManager _uiManager;
        
        public void Init()
        {
            _cam = Camera.main;
        }
        
        private void Update()
        {
            if (!_uiManager && GameSingleton.Instance && GameSingleton.Instance.uiManager) {
                _uiManager = GameSingleton.Instance.uiManager;
            }
            else if(_uiManager)
                if (_uiManager.inventoryPanel.activeSelf || _uiManager.shopPanel.activeSelf || _uiManager.upgradePanel.activeSelf || _uiManager.pausePanel.activeSelf 
                    || GameSingleton.Instance.endGamePanel.winMessage.IsActive() || GameSingleton.Instance.endGamePanel.loseMessage.IsActive()) return ;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << 8) ) {
                Vector3 hitPos = hit.point;
                float xHit = Mathf.Floor(hitPos.x);
                float zHit = Mathf.Floor(hitPos.z);
                int posX = (int) xHit;
                int posZ = (int) zHit;
                TerrainGrid.Instance.SelectZone(posZ, posX);
            }

        }
    }
}