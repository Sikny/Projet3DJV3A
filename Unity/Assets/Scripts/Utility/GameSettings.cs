using UnityEngine;

namespace Utility {
    /**
     * <summary>Global game settings</summary>
     */
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObject/GameSettings")]
    public class GameSettings : ScriptableObject {
        [Range(0f, 1f)] public float soundVolume = 0.5f;
        [Range(0f, 1f)] public float musicVolume = 0.5f;
        public Language.Language language;
        public bool invertCameraX;
        public bool invertCameraY;
    }
}