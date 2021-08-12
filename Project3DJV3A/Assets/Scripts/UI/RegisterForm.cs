using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class RegisterForm : MonoBehaviour {
        public InputField firstName;
        public InputField lastName;
        public InputField mail;
        public InputField password;
        public InputField confirmPassword;
        public Button confirmButton;
        public Text errorMessage;

        private bool PasswordsMatch() {
            return password.text == confirmPassword.text;
        }

        private bool AllFieldsFilled() {
            return firstName.text != "" && lastName.text != "" && mail.text != "" && password.text != ""
                   && confirmPassword.text != "";
        }

        public bool ValidForm() {
            if (!AllFieldsFilled()) {
                errorMessage.text = "Veuillez renseigner tous les champs";
                return false;
            }

            if (!PasswordsMatch()) {
                errorMessage.text = "Les mots de passe ne correspondent pas";
                return false;
            }
            //confirmButton.enabled = false;
            return true;
        }
    }
}
