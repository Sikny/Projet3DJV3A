using System;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration {
    [Serializable]
    public class Rule {
        public int seedWorld;    // used in free-mode(0 in level mode)

        // This following is set from .lvl file or randomize based on seedWorld in free-mode
        int maxBudget;
        int globalDifficulty;

        List<Dictionary<Vector2, float>> mapModifierHeightmap;
        List<Dictionary<Vector2, int>> localSpawnDifficulty; //SPEC : to avoid the gen into the castle
        List<Dictionary<Vector3, int>> mapCastlePiecesPlacement;
    }
}
