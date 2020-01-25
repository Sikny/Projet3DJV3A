using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule
{
    int seedWorld; // used in free-mode(0 in level mode)

    // This following is set from .lvl file or randomize based on seedWorl in free-mode
    int maxBudget;
    int globalDifficulty;

    List<Dictionary<Vector2, float>> mapModifierHeightmap;
    List<Dictionary<Vector2, int>> localSpawnDifficulty; //SPEC : to avoid the gen into the castle
    List<Dictionary<Vector3, int>> mapCastlePiecesPlacement; 

}
