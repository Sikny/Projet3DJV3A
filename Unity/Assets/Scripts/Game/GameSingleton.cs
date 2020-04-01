using Settings;
using UnityEngine;

namespace Game {
    public class GameSingleton : MonoBehaviour {
        private static GameSingleton _instance;

        public static GameSingleton Instance{
            get
            {
                if(_instance == null) _instance = new GameSingleton();
                return _instance;
            }
        }

        public GameSettings gameSettings;
    }
}
