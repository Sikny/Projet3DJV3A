﻿using System;
using TMPro;
using UnityEngine;

namespace TerrainGeneration {
    public class TerrainUnit : MonoBehaviour {
        private Color startcolor;
        public MeshRenderer renderer;
        private Camera cam;
        private Grid gridObject;

        private int maxSizeX;
        private int maxSizeZ;
        
        private void Start()
        {
            cam = Camera.main;
            gridObject = Grid.getInstance();
        }
        

        public void UpdateBounds() {
            // round corners if no neighbour
        }
        void OnMouseEnter()
        {
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                float xHit = Mathf.Floor(hit.transform.position.x);
                float zHit = Mathf.Floor(hit.transform.position.z);
                gridObject.TileX = (int)xHit + 15 ;
                gridObject.TileZ = (int)zHit + 15 ;
            }

            startcolor = renderer.material.color;
            paintCursor(gridObject.TileZ, gridObject.TileX, Color.yellow, 3);

        }
        void OnMouseExit()
        {
            paintCursor(gridObject.TileZ, gridObject.TileX, startcolor, 3);
            renderer.material.color = startcolor;
        }

        void paintCursor(int centerZ, int centerX, Color color, int cursorSize)
        {
            //Debug.Log("cursor size:" + cursorSize);
            //Debug.Log("cursor size divided" + -cursorSize/2);

            int maxZ = Grid.getInstance().GridArray.GetLength(0);
            int maxX = Grid.getInstance().GridArray.GetLength(1);
            for (int i = -cursorSize/2; i <= cursorSize/2; i++)
            {
                if((centerX-Mathf.Abs(i) < 0 && i < 0) || (centerX+Mathf.Abs(i) >= maxX && i > 0))
                    continue;
                for (int j = -cursorSize/2; j <= cursorSize/2; j++)
                {

                    if ((centerZ - Mathf.Abs(j) < 0 && j < 0)|| (centerZ + Mathf.Abs(j) >= maxZ && j > 0))
                    {
                        continue;
                    }
                    Grid.getInstance().CubeRenderers[(centerZ+j),(centerX+i)].material.color = color;
                }
            }

        }
        
    }
    


}
