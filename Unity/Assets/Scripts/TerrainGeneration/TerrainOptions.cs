using UnityEngine;

namespace TerrainGeneration {
    public class TerrainOptions : MonoBehaviour {
        public Rule rules = new Rule();
        
        [Tooltip("Number of water areas")]
        public int waterCount;
        [Tooltip("Size of each water area, recommended > terrain width")]
        public int maxWaterSize;
        public int width = 10;
        public int height = 10;

        [HideInInspector]
        public int crashLoopLimit = 1000;    // Iteration limit for stopping generation -> at least 10*waterSize
    }
}