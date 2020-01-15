using UnityEngine;
using UnityEngine.Networking;
using WebClient;

namespace Menus {
    public class MenuManagerScript : MonoBehaviour {
        public GameObject optionsPanel;
        public ConnectForm connectionPanel;
        public GameObject registerPanel;
        
        public void OnConnectConfirmPressed() {
            connectionPanel.confirmButton.enabled = false;
            StartCoroutine(ConnectModule.Instance.ConnectUser(connectionPanel.mail.text, connectionPanel.password.text, ProcessConnectionResult));
            //StartCoroutine(ConnectModule.Instance.ConnectUser());
            //  TODO CONNECT, LOADING, CONFIRM WINDOW
        }

        private void ProcessConnectionResult(UnityWebRequest www) {
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            }
            else {
                Debug.Log("Connect success");
                string result = www.downloadHandler.text;
                Debug.Log("Received: " + result);
            }
        }
    }
}
