using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility {
    public class EndGamePanel : MonoBehaviour {
        public const int ID_SCENE_MENU = 1;
        public const int ID_SCENE_GAME = 2;
    
        public TextMeshProUGUI winMessage;
        public TextMeshProUGUI loseMessage;

        public int TypeEndGame {
            get => _typeEndGame;
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
                    SceneManager.LoadScene(ID_SCENE_MENU);
                    break;
                case 1:
                    SceneManager.LoadScene(ID_SCENE_GAME);
                    break;
            }
            gameObject.SetActive(false);
        }
    }
}
