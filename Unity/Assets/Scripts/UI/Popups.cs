using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI {
    public class Popups : MonoBehaviour
    {
        public TextMeshProUGUI popupText;

        public Color defaultColor = Color.yellow;

        #region Singleton
        public static Popups instance;

        private void Awake()
        {
            // TODO INIT IN GAME SINGLETON
            if (instance != null)
            {
                #if UNITY_EDITOR
                Debug.Log("Several instances");
                #endif
                return;
            }
            instance = this;
        }
        #endregion
        
        public void Popup(String content, Color color)
        {
            popupText.color = color;
            popupText.SetText(content);
            popupText.gameObject.SetActive(true);
            StartCoroutine(DelayCorouting(3));
        }
    
        public void Popup(String content)
        {
            popupText.color = defaultColor;
            popupText.SetText(content);
            popupText.gameObject.SetActive(true);
            StartCoroutine(DelayCorouting(3));
        }

        IEnumerator DelayCorouting(int seconds)
        {
            yield return new WaitForSeconds(seconds);
        
            popupText.gameObject.SetActive(false);
        }
    }
}
