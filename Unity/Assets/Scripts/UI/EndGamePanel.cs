using TMPro;
using UnityEngine;
using Utility;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UI {
    public class EndGamePanel : MonoBehaviour {
        public TextMeshProUGUI winMessage;
        public TextMeshProUGUI loseMessage;

        public int TypeEndGame {
            set {
                _typeEndGame = value;
                switch (_typeEndGame) {
                    case 0:    // Lose
                        winMessage.gameObject.SetActive(false);
                        loseMessage.gameObject.SetActive(true);
                        break;
                    case 1:    // Win
                        winMessage.gameObject.SetActive(true);
                        loseMessage.gameObject.SetActive(false);
                        if (GameSingleton.Instance.levelManager != null)
                            GameSingleton.Instance.levelManager.NextLevel();
                        break;
                }
            }
        }
        private int _typeEndGame;

        public void CallBtn(int idBtn) {
            switch (idBtn) {
                case 0:
                    GameSingleton.Instance.sceneManager.LoadScene("Menu");
                    break;
                case 1:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;
            }
            gameObject.SetActive(false);
        }
    }
}
