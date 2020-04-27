using UnityEngine;

namespace Utility {
    /**
     * <summary>Global game variables</summary>
     */
    [CreateAssetMenu(fileName = "GameVariables", menuName = "ScriptableObject/GameVariables")]
    public class GameVariables : ScriptableObject {
        [HideInInspector] public float timeScaleGameActive = 1f;
    }
}
