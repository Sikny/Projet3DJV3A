using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Items;
using Terrain;
using UI;
using Units;
using Units.PathFinding;
using UnityEngine;
using Utility;

namespace Game {
    [Serializable]
    public class EnemySpawn {
        public float spawnTime;
        public EntityType entityType;
        public Vector2 position;
        public Sprite icon;
    }
    public class Level : MonoBehaviour {
        private Shop _shop;
        private ShopManager _shopManager;
        private SystemUnit _systemUnit;
        [HideInInspector] public AStarHandler aStarHandler;

        public Rule rule;
        public TerrainMeshBuilder terrainBuilder;
        
        [Header("Shop content")]
        public List<Consumable> consumablesList = new List<Consumable>();
        public List<Equipment> equipmentsList = new List<Equipment>();
        public List<StoreUnit> unitList = new List<StoreUnit>();
        
        [Header("Enemies")]
        public List<EnemySpawn> enemySpawns;
        public List<Transform> livingEnemies;

        private List<PlayerUnit> _playerUnits;
        private bool _levelStarted;
        
        public void Init() {
            _systemUnit = FindObjectOfType<SystemUnit>();
            StartCoroutine(terrainBuilder.Init(InitAStar));
            
            _shop = Shop.Instance;
            _shop.ClearShop();
            _shopManager = ShopManager.instance;

            
            foreach (Consumable cons in consumablesList) {
                _shop.AddConsummable(cons);
            }

            foreach (Equipment equip in equipmentsList) {
                _shop.AddEquipment(equip);
            }

            foreach (StoreUnit storeUnit in unitList) {
                _shop.AddStoreUnit(storeUnit);
            }
            _shopManager.UpdateUI(_shop);

        }
        
        private void InitAStar()
        {
            var offset = Vector3.right * (TerrainGrid.Width / 2f) +
                         Vector3.forward * (TerrainGrid.Height / 2f);
            terrainBuilder.transform.position += offset;
            _systemUnit.cam.transform.position += offset;
            aStarHandler = new AStarHandler(terrainBuilder.terrainOptions, terrainBuilder);
            /*for (int i = 0; i < aStarHandler.grid.Length; i++)
            {
                for (int j = 0; j < aStarHandler.grid[i].Length; j++)
                {
                    if (aStarHandler.grid[i][j] == 0) continue;
                    Vector3 pos = new Vector3(i, 0, j);
                    GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = pos;
                }
            }*/
        }

        private bool _gameEnded;
        private void FixedUpdate()
        {
            if (!_levelStarted) return;
            _playerUnits.Clear();
            foreach (var unit in _systemUnit.units)
            {
                if (unit.GetType() == typeof(PlayerUnit)) {
                    _playerUnits.Add((PlayerUnit) unit);
                }
            }
            if (enemySpawns.Count == 0 && livingEnemies.Count == 0 && !_gameEnded)
            {
                _gameEnded = true;
                GameSingleton.Instance.EndGame(1);    // WIN
            } else if (_playerUnits.Count == 0) {
                GameSingleton.Instance.EndGame(0);
            }
            
            for (int i = 0; i < livingEnemies.Count; i++) {
                if (livingEnemies[i] == null) {
                    livingEnemies.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < _playerUnits.Count; i++) {
                if (_playerUnits[i] == null || !_playerUnits[i].gameObject.activeSelf) {
                    _playerUnits.RemoveAt(i);
                    break;
                }
            }
        }
        
        public IEnumerator StartLevel() {
            _playerUnits = new List<PlayerUnit>();
            for (int i = enemySpawns.Count - 1; i >= 0; i--)
            {
                EnemySpawn current = enemySpawns[0];
                var offset = Vector3.right * (TerrainGrid.Width / 2f) +
                             Vector3.forward * (TerrainGrid.Height / 2f);
                Vector3 position = new Vector3(current.position.x, SystemUnit.YPos, current.position.y) + offset;
                DOVirtual.DelayedCall(current.spawnTime, () => {
                    
                    Transform newUnit = _systemUnit.SpawnUnit(current.entityType, _systemUnit.aiUnitPrefab, position);
                    livingEnemies.Add(newUnit);
                    enemySpawns.Remove(current);
                });
                yield return null;
            }
            GameSingleton.Instance.ResumeGame();
            _levelStarted = true;
        }

        private void OnDrawGizmos() {
            if (Application.isPlaying) return;
            
            // Enemy spawns
            Gizmos.color = Color.red;
            int enemyLen = enemySpawns.Count;
            for (int i = 0; i < enemyLen; i++) {
                Vector3 position = new Vector3(enemySpawns[i].position.x, SystemUnit.YPos, enemySpawns[i].position.y);
                Gizmos.DrawSphere(position, 0.5f);
            }
        }
    }
}
