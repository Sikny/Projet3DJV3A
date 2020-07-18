using System.Collections.Generic;
using UnityEngine;

namespace Utility.PoolManager {
    public class Pool {
        private List<PoolableObject> _objectsInPool;
        private PoolableObject _prefab;
        private Transform _parent;

        public void Initialize(PoolableObject parPrefabObject, int number, Transform parent) {
            _parent = parent;
            _objectsInPool = new List<PoolableObject>();
            _prefab = parPrefabObject;
            for (int i = 0; i < number; ++i) {
                PoolableObject go = Object.Instantiate(_prefab, _parent);
                go.DeInit();
                _objectsInPool.Add(go);
            }
        }

        public PoolableObject PullObject() {
            int numberInPool = _objectsInPool.Count;
            for (int i = 0; i < numberInPool; ++i) {
                if (!_objectsInPool[i].IsActive())
                    return _objectsInPool[i];
            }
            return IncreaseSize(5);
        }

        private PoolableObject IncreaseSize(int number) {
            int index = _objectsInPool.Count - 1;
            for (int i = 0; i < number; ++i) {
                PoolableObject go = Object.Instantiate(_prefab, _parent);
                go.DeInit();
                _objectsInPool.Add(go);
            }
            return _objectsInPool[index];
        }

        public void ReleaseAll() {
            for (int i = _objectsInPool.Count - 1; i >= 0; --i) {
                _objectsInPool[i].DeInit();
            }
        }
    }
}