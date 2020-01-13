using UnityEngine;

namespace Menus {
    public class MenuManagerScript : MonoBehaviour {
        public GameObject optionsPanel;
        public GameObject connectionPanel;
        public GameObject registerPanel;
        public void OnConnectButtonPressed() {
            connectionPanel.SetActive(true);
        }

        public void OnOptionsButtonPressed() {
            optionsPanel.SetActive(true);
        }

        public void OnOptionsBackButtonPressed() {
            optionsPanel.SetActive(false);
        }

        public void OnConnectConfirmPressed() {
            //StartCoroutine(ConnectModule.Instance.ConnectUser());
            //  TODO CONNECT, LOADING, CONFIRM WINDOW
        }
    }
}
