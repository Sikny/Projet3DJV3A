using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {
    public const int ID_SCENE_MENU = 0;
    public const int ID_SCENE_GAME = 1;
    
    public TextMeshProUGUI msgTextMesh;

    public static int typeEndGame;
    
    // Start is called before the first frame update
    void Start() {
        switch (typeEndGame) {
            case 0:
                msgTextMesh.SetText("Game Over");
                msgTextMesh.color = Color.red;
                break;
            case 1:
                msgTextMesh.SetText("You won !");
                msgTextMesh.color = Color.green;
                break;
        }
    }

    public void callBtn(int idBtn) {
        switch (idBtn) {
            case 0:
                SceneManager.LoadScene(ID_SCENE_MENU);
                break;
            case 1:
                SceneManager.LoadScene(ID_SCENE_GAME);
                break;
        }
    }

    public static void setTypeEndGame(int type) {
        typeEndGame = type;
    }
}
