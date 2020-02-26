using UnityEngine;
using UnityEngine.Networking;
using WebClient;

namespace Menus {
    public class MenuManagerScript : MonoBehaviour {
        public GameObject optionsPanel;
        public ConnectForm connectionPanel;
        public RegisterForm registerPanel;
        
        public void OnConnectConfirmPressed() {
            if(connectionPanel.ValidForm()) {
                StartCoroutine(ConnectModule.Instance.ConnectUser(connectionPanel.mail.text, 
                    connectionPanel.password.text, ProcessConnectionResult));
                //  TODO CONNECT, LOADING, CONFIRM WINDOW
            }
        }

        public void OnRegisterConfirmPressed() {
            if (registerPanel.ValidForm()) {
                StartCoroutine(ConnectModule.Instance.RegisterUser(registerPanel.firstName.text,
                    registerPanel.lastName.text, registerPanel.mail.text, registerPanel.password.text,
                    ProcessConnectionResult));
                //  TODO CONNECT, LOADING, CONFIRM WINDOW
            }
        }

        public void OnExitPressed() {
            Application.Quit();
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
