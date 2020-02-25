using UnityEngine;

namespace Game {
    /**
     * <summary>Global game settings</summary>
     */
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
    public class GameSettings : ScriptableObject {
        public float soundVolume;
        public float musicVolume;
        public Language.Language language;
        public bool invertCameraX;
    }
}
