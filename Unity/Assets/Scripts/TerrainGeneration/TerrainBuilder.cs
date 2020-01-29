using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TerrainGeneration {
    public class TerrainBuilder : MonoBehaviour {
        public TerrainOptions terrainOptions;
        public UnitDict unitDict;
    
        private float unitScale = 1;

        private readonly List<KeyValuePair<ZoneType, UnitList>> _terrainData = new List<KeyValuePair<ZoneType, UnitList>>();
        
        private int[,] grid;
        private String printGrid = "\n";

        //private Grid grid;

        private void Start() {
            grid = new int[terrainOptions.height, terrainOptions.width];
            BuildArray();
            BuildTerrain();
        }
    
        /**
         * Array to gameObjects
         */
        private void BuildTerrain() {
            Vector3 position;
            float xModifier = -unitScale * (terrainOptions.width - 1) / 2;
            float yModifier = -unitScale * (terrainOptions.height - 1) / 2;
            // basic fill
            for (int i = 0; i < terrainOptions.width; i++) {
                for (int j = 0; j < terrainOptions.height; j++) {
                    grid[j, i] = 0;
                    position = transform.position + new Vector3(unitScale*i+xModifier, 0,
                                   unitScale*j+yModifier);
                    GameObject unit = Instantiate(unitDict.GetPrefab(ZoneType.Grass), position, Quaternion.identity);
                    unit.transform.SetParent(transform);
                }
            }

            foreach (var value in _terrainData) {
                int listLen = value.Value.Count();
                for (int i = 0; i < listLen; i++) {
                    position = transform.position + new Vector3(unitScale * value.Value.Get(i).x+xModifier, 0,
                                   unitScale * value.Value.Get(i).y+yModifier);
                    GameObject unit = Instantiate(unitDict.GetPrefab(value.Key), position, Quaternion.identity);
                    unit.transform.SetParent(transform);
                }
            }
        }
        
        /**
         * Builds water areas into array
         */
        private void BuildArray() {
            Random.InitState(terrainOptions.rules.seedWorld);
            // build water areas
            for (int i = 0; i < terrainOptions.waterCount; i++) {
                BuildOneWaterArea();
            }
        }

        private void BuildOneWaterArea() {
            Vector2Int tmp;
            int randomX, randomY;
            UnitList waterList = new UnitList();
            int crashHandler = 0;
            while (waterList.Count() < terrainOptions.maxWaterSize 
                   && crashHandler < terrainOptions.crashLoopLimit) {
                crashHandler++;
                if (waterList.Count() == 0) {
                    // Horizontal
                    randomY = Random.Range(0, terrainOptions.width);
                    // Vertical
                    randomX = Random.Range(0, terrainOptions.height);
                    if (randomX > 0 && randomX < terrainOptions.width - 1 
                            && randomY > 0 && randomY < terrainOptions.height - 1)
                        continue;
                } else {
                    tmp = waterList.Last();
                    randomY = Random.Range(tmp.y - 1, tmp.y + 2);
                    randomX = Random.Range(tmp.x - 1, tmp.y + 2);
                }
                if (waterList.Contains(randomX, randomY) || randomX < 0 || randomY < 0
                    || randomX >= terrainOptions.width || randomY >= terrainOptions.height
                    || !waterList.HasNeighbour(randomX, randomY))
                    continue;
                waterList.Add(randomX, randomY);
                grid[randomY, randomX] = 1;
            }
            _terrainData.Add(new KeyValuePair<ZoneType, UnitList>(ZoneType.Water, waterList));
        }
       
        private void DisplayGrid()
        {
            for (int i = 0; i < terrainOptions.height; i++) {
                for (int j = 0; j < terrainOptions.width; j++)
                {
                    printGrid += grid[terrainOptions.height-i-1, j];
                }

                printGrid += "\n";
            }
            Debug.Log(printGrid);
        }
    }
}

