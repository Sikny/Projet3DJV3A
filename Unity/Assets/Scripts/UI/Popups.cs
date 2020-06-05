using System;
using System.Collections;
using Game;
using TMPro;
using UnityEngine;
using Utility;

namespace UI {
    public class Popups : MonoBehaviour
    {
        public TextMeshProUGUI popupText;

        public Color defaultColor = Color.yellow;

        #region Singleton
    
        public static Popups instance;
    
    
        private void Awake()
        {
            if (instance != null)
            {
                Debug.Log("Several instances");
                return;
            }
            instance = this;
        }

        #endregion

        private void Start()
        {
            GameSingleton.Instance.soundManager.Play("Level theme");
        }

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
