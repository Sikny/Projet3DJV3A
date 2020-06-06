using UnityEngine;

namespace Utility {
    /**
     * <summary>Global game settings</summary>
     */
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObject/GameSettings")]
    public class GameSettings : ScriptableObject {
        public float soundVolume;
        public float musicVolume;
        public Language.Language language;
        public bool invertCameraX;
        public bool invertCameraY;

    }
}