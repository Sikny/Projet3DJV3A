﻿using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration {
    public class TerrainOptions : MonoBehaviour {
        public Rule rules = new Rule();
        
        [Tooltip("Number of water areas")]
        public int waterCount;
        [Tooltip("Size of each water area, recommended > terrain width")]
        public int maxWaterSize;

        public int mountainCount;
        public int minMountainHeight;
        public int maxMountainHeight;

        public int width = 10;
        public int height = 10;

        public Dictionary<Vector2, float> modifierHeightMap = new Dictionary<Vector2, float>();

        [HideInInspector]
        public int crashLoopLimit = 1000;    // Iteration limit for stopping generation -> at least 10*waterSize
    }
}