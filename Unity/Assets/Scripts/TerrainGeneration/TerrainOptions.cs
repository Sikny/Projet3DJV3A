using UnityEngine;

namespace TerrainGeneration {
    public class TerrainOptions : MonoBehaviour {
        [Tooltip("Number of water areas")]
        public int waterCount;
        [Tooltip("Size of each water area, recommended > terrain width")]
        public int maxWaterSize;
        public int width = 10;
        public int height = 10;
        public int seed = 0;

        [HideInInspector]
        public int crashLoopLimit = 1000;    // Iteration limit for stopping generation -> at least 10*waterSize
    }
}