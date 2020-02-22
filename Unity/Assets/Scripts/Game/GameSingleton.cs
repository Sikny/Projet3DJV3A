using Language;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game {
    /**
     * <summary>Game Manager, should handle scenes transitions</summary>
     */
    public class GameSingleton : MonoBehaviour {
        private static GameSingleton _instance = null;
        public static GameSingleton Instance => _instance;

        public UnityEvent updateLoop;
        public UnityEvent fixedUpdateLoop;
        public UnityEvent lateUpdateLoop;

        public GameSettings gameSettings;
        public LanguageDictionary languageDictionary;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

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

        // dont make private, used by events
        public void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }
    }
}
