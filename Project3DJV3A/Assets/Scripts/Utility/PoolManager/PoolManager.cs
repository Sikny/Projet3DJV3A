using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.PoolManager {
    public class PoolManager : MonoBehaviour {
        public static PoolManager Instance() { return _instance; }

        private static PoolManager _instance;

        private void Awake() {
            _instance = this;
            Initialize();
            DontDestroyOnLoad(this);
        }

        public List<ObjectForPool> objectPrefabs;
        private Dictionary<Type, Pool> _pools;

        private void Initialize() {
            _pools = new Dictionary<Type, Pool>();
            foreach (ObjectForPool obj in objectPrefabs) {
                Pool newPool = new Pool();
                Transform parent = new GameObject(obj.prefab.GetType().ToString()).transform;
                parent.SetParent(transform);
                newPool.Initialize(obj.prefab, obj.number, parent);
                _pools.Add(obj.prefab.GetType(), newPool);
            }
        }

        public PoolableObject GetPoolableObject(Type objectType) {
            return _pools[objectType].PullObject();
        }

        public void ReleasePooledObject(PoolableObject obj) {
            obj.DeInit();
        }

        // On scene change
        public void ReleaseAll() {
            foreach (var pair in _pools) {
                pair.Value.ReleaseAll();
            }
        }
    }
}