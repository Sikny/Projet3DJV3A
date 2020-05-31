using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Items;
using Terrain;
using Units;
using UnityEngine;
using Utility;

namespace Game {
    [Serializable]
    public class EnemySpawn {
        public float spawnTime;
        public EntityType entityType;
        public Vector2 position;
    }
    public class Level : MonoBehaviour {
        private Shop _shop;
        private SystemUnit _systemUnit;

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
            StartCoroutine(terrainBuilder.Init());

            _systemUnit = FindObjectOfType<SystemUnit>();
            
            _shop = Shop.Instance;
            _shop.ClearShop();
            
            foreach (Consumable cons in consumablesList) {
                _shop.AddConsummable(cons);
            }

            foreach (Equipment equip in equipmentsList) {
                _shop.AddEquipment(equip);
            }

            foreach (StoreUnit storeUnit in unitList) {
                _shop.AddStoreUnit(storeUnit);
            }
        }

        private void Update() {
            if (!_levelStarted) return;
            if (enemySpawns.Count == 0 && livingEnemies.Count == 0) {
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
            _playerUnits = new List<PlayerUnit>(FindObjectsOfType<PlayerUnit>());
            while (enemySpawns.Count > 0) {
                EnemySpawn current = enemySpawns[0];
                Vector3 position = new Vector3(current.position.x, SystemUnit.YPos, current.position.y);
                DOVirtual.DelayedCall(current.spawnTime, () => {
                    Transform newUnit = _systemUnit.SpawnUnit(current.entityType, _systemUnit.aiUnitPrefab, position);
                    livingEnemies.Add(newUnit);
                    enemySpawns.Remove(current);
                });
                yield return 0;
            }
            GameSingleton.Instance.ResumeGame();
            _levelStarted = true;
        }

        private void OnDrawGizmos() {
            if (Application.isPlaying) return;
            
            Gizmos.color = Color.red;
            int enemyLen = enemySpawns.Count;
            for (int i = 0; i < enemyLen; i++) {
                Vector3 position = new Vector3(enemySpawns[i].position.x, SystemUnit.YPos, enemySpawns[i].position.y);
                Gizmos.DrawSphere(position, 0.5f);
            }
        }
    }
}
