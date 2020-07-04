using TMPro.EditorUtilities;
using UI;
using UnityEngine;
using Utility;

namespace Terrain {
    public class TerrainRaycaster : MonoBehaviour {
        private Camera _cam;
        public int height;
        public int width;
        private UiManager _uiManager;
        private void Awake()
        {
            _uiManager = GameSingleton.Instance.uiManager;
            _cam = Camera.main;
            
        }
        
        
        private void Update()
        {
            if (_uiManager.inventoryPanel.activeSelf || _uiManager.shopPanel.activeSelf || _uiManager.upgradePanel.activeSelf || _uiManager.pausePanel.activeSelf) return ;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << 8) ) {
                Vector3 hitPos = hit.point;
                float xHit = Mathf.Floor(hitPos.x);
                float zHit = Mathf.Floor(hitPos.z);
                int posX = (int) xHit + width/2;
                int posZ = (int) zHit + height/2;
                TerrainGrid.Instance.SelectZone(posZ, posX);
            }

        }
    }
}