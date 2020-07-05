using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI {
    public class Popups : MonoBehaviour
    {
        public TextMeshProUGUI popupText;
        public TextMeshProUGUI topPopupText;
        public Color defaultColor = Color.yellow;

        #region Singleton
        public static Popups instance;

        private void Awake()
        {
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
            StartCoroutine(DelayCorouting(3,popupText));
        }
    
        public void Popup(String content)
        {
            popupText.color = defaultColor;
            popupText.SetText(content);
            popupText.gameObject.SetActive(true);
            StartCoroutine(DelayCorouting(3,popupText));
        }
        
        public void PopupTop(String content, Color color)
        {
            topPopupText.color = color;
            topPopupText.SetText(content);
            topPopupText.gameObject.SetActive(true);
            StartCoroutine(DelayCorouting(3,topPopupText));
        }
    
        public void PopupTop(String content)
        {
            topPopupText.color = defaultColor;
            topPopupText.SetText(content);
            topPopupText.gameObject.SetActive(true);
            StartCoroutine(DelayCorouting(3,topPopupText));
        }

        IEnumerator DelayCorouting(int seconds, TextMeshProUGUI text)
        {
            yield return new WaitForSeconds(seconds);
        
            text.gameObject.SetActive(false);
        }
        
        
    }
}
