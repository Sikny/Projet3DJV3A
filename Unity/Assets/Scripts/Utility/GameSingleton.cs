using Language;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Utility {
    /**
     * <summary>Game Manager, should handle scenes transitions</summary>
     */
    public class GameSingleton : MonoBehaviour {
        private static GameSingleton _instance;
        public static GameSingleton Instance => _instance;

        public UnityEvent updateLoop;
        public UnityEvent fixedUpdateLoop;
        public UnityEvent lateUpdateLoop;
        
        [HideInInspector] public SceneManager sceneManager;
        public GameVariables gameVariables;
        [HideInInspector] public LevelManager levelManager;

        public GameSettings gameSettings;
        public LanguageDictionary languageDictionary;

        public EndGamePanel endGamePanel;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            sceneManager = new SceneManager();
            sceneManager.LoadScene("Menu");
        }
        
        // optimizations purposes
        private void Update() {
            updateLoop.Invoke();
        }
        private void FixedUpdate() {
            fixedUpdateLoop.Invoke();
        }
        private void LateUpdate() {
            lateUpdateLoop.Invoke();
        }

        // Do not delete or make private (used by events)
        public void LoadScene(string sceneName) {
            sceneManager.LoadScene(sceneName);
        }

        public void StartFight() {
            if (levelManager != null) {
                StartCoroutine(levelManager.loadedLevel.StartLevel());
            } else {
                // Wave mode ?
            }
        }

        public void EndGame(int status) {
            endGamePanel.TypeEndGame = status;
            endGamePanel.gameObject.SetActive(true);
        }
    }
}
