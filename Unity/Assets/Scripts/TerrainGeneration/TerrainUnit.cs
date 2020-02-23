using System;
using UnityEngine;

namespace TerrainGeneration {
    public class TerrainUnit : MonoBehaviour {
        public MeshRenderer meshRenderer;

        private Color _startColor;
        private Camera cam;
        private int maxSizeX;
        private int maxSizeZ;
        
        private void Awake() {
            cam = Camera.main;
        }
        
        void OnMouseEnter() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100)) {
                Vector3 hitPos = hit.transform.position;
                float xHit = Mathf.Floor(hitPos.x);
                float zHit = Mathf.Floor(hitPos.z);
                int posX = (int)xHit + 15 ;
                int posZ = (int)zHit + 15 ;
                TerrainGrid.Instance.SelectZone(posZ, posX);
            }
        }

        public void SelectUnit() {
            Material mat = meshRenderer.material;
            _startColor = mat.color;
            mat.color = Color.yellow;
        }

        public void DeselectUnit() {
            meshRenderer.material.color = _startColor;
        }
    }
}
