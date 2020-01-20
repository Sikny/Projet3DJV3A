using UnityEngine;

namespace Settings {
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
    public class GameSettings : ScriptableObject {
        private static GameSettings _instance;
        public static GameSettings Instance {
            get {
                if(!_instance) _instance = CreateInstance<GameSettings>();
                return _instance;
            }
        }
        
        public float soundVolume;
        public float musicVolume;
        public Language.Language language;
    }
}
