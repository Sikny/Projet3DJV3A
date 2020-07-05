using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utility;
using WebClient;

namespace UI {
    public class MenuManager : MonoBehaviour {
        public GameObject optionsPanel;
        public ConnectForm connectionPanel;
        public RegisterForm registerPanel;
        public GameObject background;
        
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
                    registerPanel.lastName.text, registerPanel.mail.text, registerPanel.password.text,registerPanel.confirmPassword.text,
                    ProcessRegisterResult));
                //  TODO CONNECT, LOADING, CONFIRM WINDOW
            }
        }

        public void OnExitPressed() {
            Application.Quit();
        }

        private void ProcessConnectionResult(UnityWebRequest www) {
            if (www.isNetworkError || www.isHttpError) {
            }
            else {
                string result = www.downloadHandler.text;
                if (result.Contains("NOK"))
                {
                    GameObject password = connectionPanel.transform.Find("PasswordInput").gameObject;
                    InputField passwordIn = password.GetComponent<InputField>();
                    passwordIn.text = "";
                    Popups.instance.Popup("Wrong identifications!", Color.red);
                }
                else
                {
                    GameSingleton.Instance.GetPlayer().token = result;
                    PlayerPrefs.Save();
                    connectionPanel.gameObject.SetActive(false);
                    background.SetActive(false);
                }
            }
        }
        
        private void ProcessRegisterResult(UnityWebRequest www) {
            if (www.isNetworkError || www.isHttpError) {
                
            }
            else {
                string result = www.downloadHandler.text;
                if (result.Equals("OK"))
                {
                    registerPanel.gameObject.SetActive(false);
                    background.SetActive(false);
                }
                else if (result.Equals("NOK-MAIL-ALREADY-USED"))
                {
                    Popups.instance.Popup("Mail is already used!", Color.red);
                }
                else
                {
                    //todo
                }
            }
            
        }
    }
}
