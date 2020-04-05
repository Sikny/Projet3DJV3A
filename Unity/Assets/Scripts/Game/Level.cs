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

        public TerrainBuilder terrainBuilder;

        public List<Consummable> consumablesList = new List<Consummable>();
        public List<Equipment> equipmentsList = new List<Equipment>();
        public List<StoreUnit> unitList = new List<StoreUnit>();

        public List<EnemySpawn> enemySpawns;
        
        public void Init() {
            StartCoroutine(terrainBuilder.Init());
            
            _shopContent = ShopContent.Instance;
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
        
        public void StartLevel() {
            while (enemySpawns.Count > 0) {
                DOVirtual.DelayedCall(enemySpawns[0].spawnTime, () => {
                    
                });
                enemySpawns.RemoveAt(0);
            }
        }
    }
}
