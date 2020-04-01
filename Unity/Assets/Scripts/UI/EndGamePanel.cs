using Game;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class EndGamePanel : MonoBehaviour {
        public TextMeshProUGUI winMessage;
        public TextMeshProUGUI loseMessage;

        public int TypeEndGame {
            set {
                _typeEndGame = value;
                switch (_typeEndGame) {
                    case 0:
                        winMessage.gameObject.SetActive(false);
                        loseMessage.gameObject.SetActive(true);
                        break;
                    case 1:
                        winMessage.gameObject.SetActive(true);
                        loseMessage.gameObject.SetActive(false);
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
