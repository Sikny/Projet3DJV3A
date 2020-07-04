using UnityEngine;

namespace Utility {
    /**
     * <summary>Global game settings</summary>
     */
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObject/GameSettings")]
    public class GameSettings : ScriptableObject {
        [Range(0f, 1f)] public float soundVolume;
        [Range(0f, 1f)] public float musicVolume;
        public Language.Language language;
        public bool invertCameraX;
        public bool invertCameraY;

    }
}