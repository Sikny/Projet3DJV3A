using System;
using System.Collections.Generic;
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

        private void Start() {
            TerrainGrid.Height = terrainOptions.height;
            TerrainGrid.Width = terrainOptions.width;
            
            grid = new int[terrainOptions.height, terrainOptions.width];
            
            BuildArray();
            BuildTerrain();
        }
    
        /**
         * Array to gameObjects
         */
        private void BuildTerrain() {
            Vector3 position;
            float xModifier = -unitScale*terrainOptions.width/2f + unitScale/2f;//-unitScale * (terrainOptions.width - 1) / 2f;
            float yModifier = -unitScale*terrainOptions.height/2f + unitScale/2f;
            // basic fill
            for (int i = 0; i < terrainOptions.width; i++) {
                for (int j = 0; j < terrainOptions.height; j++) {
                    grid[j, i] = 0;
                    Vector2Int pos2d = new Vector2Int((int) (unitScale*i), (int) (unitScale*j));
                    position = transform.position + new Vector3(pos2d.x+xModifier, 0, pos2d.y+yModifier);
                    position.y = CalculateHeight(pos2d, position);
                    TerrainUnit unit = Instantiate(unitDict.GetPrefab(ZoneType.Grass), position, Quaternion.identity);
                    unit.transform.SetParent(transform);
                    
                    TerrainGrid.Instance.AddTerrainUnit(unit, j, i);
                }
            }

            foreach (var value in _terrainData) {
                int listLen = value.Value.Count();
                for (int i = 0; i < listLen; i++) {
                    position = transform.position + new Vector3(unitScale * value.Value.Get(i).x+xModifier, 0,
                                   unitScale * value.Value.Get(i).y+yModifier);
                    TerrainUnit unit = Instantiate(unitDict.GetPrefab(value.Key), position, Quaternion.identity);
                    unit.transform.SetParent(transform);
                }
            }
        }

        private float CalculateHeight(Vector2Int pos, Vector3 unitPos) {
            if (terrainOptions.modifierHeightMap.ContainsKey(pos)) {
                Vector3 position = unitPos;
                float value = terrainOptions.modifierHeightMap[pos];
                for (float i = value-1; i >= 0; i--) {
                    position.y = i;
                    TerrainUnit unit = Instantiate(unitDict.GetPrefab(ZoneType.Grass), position, Quaternion.identity);
                    unit.transform.SetParent(transform);
                }
                return terrainOptions.modifierHeightMap[pos];
            }
            return 0;
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
            // build mountains
            for (int i = 0; i < terrainOptions.mountainCount; i++) {
                BuildOneMountain();
            }
            DisplayGrid();
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
            
            TerrainGrid.Instance.GridArray = grid;
        }
       
        private void DisplayGrid() {
            for (int i = 0; i < terrainOptions.height; i++) {
                for (int j = 0; j < terrainOptions.width; j++) {
                    printGrid += grid[terrainOptions.height-i-1, j];
                }
                printGrid += "\n";
            }
            Debug.Log(printGrid);
        }

        private void BuildOneMountain() {
            int posX = Random.Range(0, terrainOptions.width);
            int posZ = Random.Range(0, terrainOptions.height);
            Vector2 mountainSource = new Vector2(posX, posZ);
            int mountTipHeight = Random.Range(terrainOptions.minMountainHeight, terrainOptions.maxMountainHeight + 1);
            if(!terrainOptions.modifierHeightMap.ContainsKey(mountainSource))
                terrainOptions.modifierHeightMap.Add(mountainSource, mountTipHeight);
            int radius = 1;
            for (int i = mountTipHeight-1; i > 0; i--) {
                SetMountainFloor(i, mountainSource, radius++);
            }
        }

        private void SetMountainFloor(int yPos, Vector2 center, int radius) {
            int y = yPos;
            for (float x = center.x - radius; x <= center.x + radius; x++) {
                for (float z = center.y - radius; z <= center.y + radius; z++) {
                    if (Random.Range(0f, 1f) > 0.6f) y = yPos-1;
                    Vector2 currentUnit = new Vector2(x, z);
                    if (!terrainOptions.modifierHeightMap.ContainsKey(currentUnit)) {
                        terrainOptions.modifierHeightMap.Add(currentUnit, y);
                    }
                    y = yPos;
                }
            }
        }
    }
}