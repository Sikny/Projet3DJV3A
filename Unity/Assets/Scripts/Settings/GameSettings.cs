using UnityEngine;

namespace Settings {
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
    public class GameSettings : ScriptableObject {
        public float soundVolume;
        public float musicVolume;
        public Language.Language language;
        public bool invertCameraX;
    }
}
