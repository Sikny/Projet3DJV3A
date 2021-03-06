﻿using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ConnectForm : MonoBehaviour {
        public InputField mail;
        public InputField password;
        public Button confirmButton;

        private bool AllFieldsFilled() {
            return mail.text != "" && password.text != "";
        }

        public bool ValidForm() {
            if (!AllFieldsFilled()) return false;
            //confirmButton.enabled = false;
            return true;
        }
    }
}
