using System;
using UnityEngine;

namespace TerrainGeneration {
    public class TerrainUnit : MonoBehaviour {
        private Color startcolor;
        public MeshRenderer renderer;
        public Camera cam;
        private Grid gridObject;

        private void Start()
        {
            gridObject = Grid.getInstance();
        }
        

        public void UpdateBounds() {
            // round corners if no neighbour
        }
        void OnMouseEnter()
        {
            //get grid coordinates 
            
            /*Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                float xHit = Mathf.Floor(hit.transform.position.x);
                float zHit = Mathf.Floor(hit.transform.position.z);
                gridObject.TileX = (int)xHit + 15 ;
                gridObject.TileZ = (int)zHit + 15 ;
                
            }*/

            startcolor = renderer.material.color;
            paintCursor(gridObject.TileZ, gridObject.TileX, Color.yellow, 3);
            Debug.Log("tileX from terrainUnit: " + gridObject.TileX);

            //renderer.material.color = Color.yellow;
        }
        void OnMouseExit()
        {
            renderer.material.color = startcolor;
        }

        void paintCursor(int centerZ, int centerX, Color color, int cursorSize)
        {
            Debug.Log("cursor size:" + cursorSize);
            Debug.Log("cursor size divided" + -cursorSize/2);
            for (int i = -cursorSize/2; i <= cursorSize/2; i++)
            {
                for (int j = -cursorSize/2; j <= cursorSize/2; j++)
                {
                    Debug.Log("changing coordinates:" + (centerZ+j) + " : " + (centerX+i));
                    Debug.Log("");
                    Grid.getInstance().CubeRenderers[(centerZ+j),(centerX+i)].material.color = color;
                }
            }

        }
    }
    


}
