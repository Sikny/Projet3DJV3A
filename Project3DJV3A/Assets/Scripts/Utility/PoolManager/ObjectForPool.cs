using System;

namespace Utility.PoolManager {
    [Serializable]
    public struct ObjectForPool {
        public PoolableObject prefab;
        public int number;
    }
}