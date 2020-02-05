using System;
using UnityEngine;

namespace TerrainGeneration {
    public class TerrainUnit : MonoBehaviour {
        private Color _startColor;
        public MeshRenderer renderer;
        

        public void UpdateBounds() {
            // round corners if no neighbour
        }
        void OnMouseEnter()
        {
            _startColor = renderer.material.color;
            renderer.material.color = Color.yellow;
        }
        void OnMouseExit()
        {
            renderer.material.color = _startColor;
        }
    }
    


}
