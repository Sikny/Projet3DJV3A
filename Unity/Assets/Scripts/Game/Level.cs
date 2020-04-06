using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Items;
using TerrainGeneration;
using Units;
using UnityEngine;

namespace Game {
    [Serializable]
    public class EnemySpawn {
        public float spawnTime;
        public EntityType entityType;
        public Vector3 position;
    }
    public class Level : MonoBehaviour {
        private ShopContent _shopContent;
        private SystemUnit _systemUnit;

        public TerrainBuilder terrainBuilder;

        public List<Consummable> consumablesList = new List<Consummable>();
        public List<Equipment> equipmentsList = new List<Equipment>();
        public List<StoreUnit> unitList = new List<StoreUnit>();

        public List<EnemySpawn> enemySpawns;
        public List<Transform> livingEnemies;
        
        public void Init() {
            StartCoroutine(terrainBuilder.Init());

            _systemUnit = FindObjectOfType<SystemUnit>();
            
            _shopContent = ShopContent.Instance;
            _shopContent.ClearShop();
            
            foreach (Consummable cons in consumablesList) {
                _shopContent.AddConsummable(cons);
            }

            foreach (Equipment equip in equipmentsList) {
                _shopContent.AddEquipment(equip);
            }

            foreach (StoreUnit storeUnit in unitList) {
                _shopContent.AddStoreUnit(storeUnit);
            }
        }

        private void Update() {
            for (int i = 0; i < livingEnemies.Count; i++) {
                if (livingEnemies[i] == null) {
                    livingEnemies.RemoveAt(i);
                    break;
                }
            }
        }
        
        public IEnumerator StartLevel() {
            while (enemySpawns.Count > 0) {
                print("Spawn enemy");
                EnemySpawn current = enemySpawns[0];
                DOVirtual.DelayedCall(current.spawnTime, () => {
                    Transform newUnit = _systemUnit.SpawnUnit(current.entityType, _systemUnit.aiUnitPrefab, current.position);
                    livingEnemies.Add(newUnit);
                    enemySpawns.Remove(current);
                });
                yield return 0;
            }
        }

        private void OnDrawGizmos() {
            if (Application.isPlaying) return;
            Gizmos.color = Color.green;
            Vector3 dims = new Vector3(terrainBuilder.unitScale*terrainBuilder.terrainOptions.width, 
                terrainBuilder.unitScale, terrainBuilder.unitScale*terrainBuilder.terrainOptions.height);
            Gizmos.DrawCube(transform.position, dims);
            
            Gizmos.color = Color.red;
            int enemyLen = enemySpawns.Count;
            for (int i = 0; i < enemyLen; i++) {
                Gizmos.DrawSphere(enemySpawns[i].position, 0.5f);
            }
        }
    }
}
