using Language;
using TMPro;
using UnityEngine;
using Utility;

namespace UI {
    public class ConnecteMode : MonoBehaviour
    {
        public TMP_Text connecteIndication;
        // Start is called before the first frame update
        void Start()
        {
            UpdateConnecteIndication();
        }
        public void UpdateConnecteIndication()
        {
            if (connecteIndication != null)
            {
                string token = GameSingleton.Instance.GetPlayer().token;
                if (string.IsNullOrEmpty(token) || token.Length < 8)
                {
                    connecteIndication.text = Traducer.Translate("Not connected");
                    connecteIndication.color = Color.white;
                }
                else
                {
                    connecteIndication.text = Traducer.Translate("Connected");
                    connecteIndication.color = Color.green;
                }
            }
        }

        public void Update()
        {
            UpdateConnecteIndication();
        }
    }
}
