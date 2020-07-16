using UnityEngine;
using UnityEngine.Networking;
using Utility;
using WebClient;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Game.arcade {
    public class TokenManager : MonoBehaviour
    {

        public bool cacheValid = false;
        public string nextAction;
    
        // Start is called before the first frame update

        public void CheckToken(string token, string next)
        {
            nextAction = next;
            StartCoroutine(ConnectModule.Instance.ValidToken(token,
                ProcessTokenResult));
        }
        private void ProcessTokenResult(UnityWebRequest www) {
            if (www.isNetworkError || www.isHttpError) {
            
            }
            else {
                string result = www.downloadHandler.text;
                if (result.Equals("OK"))
                {
                    cacheValid = true;
                    switch (nextAction)
                    {
                        case "scene.load.freeMode":
                            LoadFreeMode();
                            break;
                    }
                }
                else
                {
                    cacheValid = false;
                    GameSingleton.Instance.GetPlayer().token = "";
                    PlayerPrefs.Save();
                }
            }
            
        }

        private void LoadFreeMode()
        {
        
            GameSingleton.Instance.GetPlayer().currentScore = 0;
            GameSingleton.Instance.GetPlayer().gamemode = Player.Gamemode.ARCADE;
            UnitySceneManager.LoadScene(5);
            cacheValid = false;
            nextAction = "";
        }
    }
}
