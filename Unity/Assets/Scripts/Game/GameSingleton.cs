using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game {
    /**
     * <summary>Game Manager, should handle scenes transitions</summary>
     */
    public class GameSingleton : MonoBehaviour {
        private static GameSingleton _instance;
        public static GameSingleton Instance {
            get {
                // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
                if(_instance == null) _instance = new GameSingleton();
                return _instance;
            }
        }

        public UnityEvent updateLoop;
        public UnityEvent fixedUpdateLoop;
        public UnityEvent lateUpdateLoop;

        public GameSettings gameSettings;

        private void Awake() {
            DontDestroyOnLoad(this);

            LoadScene("Menu");
        }
        private void Update() {
            updateLoop.Invoke();
        }
        private void FixedUpdate() {
            fixedUpdateLoop.Invoke();
        }
        private void LateUpdate() {
            lateUpdateLoop.Invoke();
        }

        public void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }
    }
}
