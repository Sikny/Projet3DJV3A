using CustomEvents;
using Game;
using Language;
using Sounds;
using UI;
using Units.PathFinding;
using UnityEngine;

namespace Utility {
    /**
     * <summary>Game Manager, should handle scenes transitions</summary>
     */
    public class GameSingleton : MonoBehaviour {
        private static GameSingleton _instance;
        public static GameSingleton Instance => _instance;

        [Header("Events")] public CustomEvent updateLoop;
        public CustomEvent fixedUpdateLoop;
        public CustomEvent lateUpdateLoop;

        [HideInInspector] public SceneManager sceneManager;
        [Space] public GameVariables gameVariables;
        [HideInInspector] public LevelManager levelManager;

        public GameSettings gameSettings;
        public LanguageDictionary languageDictionary;

        [Space] public EndGamePanel endGamePanel;

        public StoreUnitList storeUnitList;
        [Space] [HideInInspector] public bool gamePaused;

        private Player _player;

        public EntityTypeToSprite entityTypeToSprite;

        //public string tokenConnection;

        public UiManager uiManager;
        public SoundManager soundManager;
        public TokenManager tokenManager;

        public AStarHandler aStarHandler;

        public string filename;
        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            
#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
#endif
            soundManager.Init();
            _player = new Player();
            sceneManager = new SceneManager();
            sceneManager.LoadScene("Menu");
        }


        // optimizations purposes
        private void Update() {
            updateLoop.Raise();
        }

        private void FixedUpdate() {
            fixedUpdateLoop.Raise();
        }

        private void LateUpdate() {
            lateUpdateLoop.Raise();
        }

        // Do not delete or make private (used by events)
        public void LoadScene(string sceneName) {
            sceneManager.LoadScene(sceneName);
        }


        public void StartFight() {
            if (levelManager != null) {
                //start fight sound play here
                StartCoroutine(levelManager.loadedLevel.StartLevel());
            }
        }

        public Player GetPlayer() {
            return _player;
        }

        public void EndGame(int status) {
            endGamePanel.TypeEndGame = status;
            endGamePanel.gameObject.SetActive(true);
            _player.Save();
        }

        public void PauseGame() {
            Time.timeScale = 0f;
            gamePaused = true;
        }

        public void ResumeGame() {
            Time.timeScale = gameVariables.timeScaleGameActive;
            gamePaused = false;
        }

        public void QuitGame() {
            Application.Quit();
        }
    }
}