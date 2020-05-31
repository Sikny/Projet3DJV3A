using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Language {
    public class Traducer : MonoBehaviour {
        public List<TextMeshProUGUI> notTranslatedTmp;
        public List<Text> notTranslated;

        private void Awake() {
            TranslateView();
        }

        public void TranslateView() {
            TextMeshProUGUI[] tMPs = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
            int tMPsCount = tMPs.Length;
            for (int i = 0; i < tMPsCount; i++) {
                if (!notTranslatedTmp.Contains(tMPs[i])) {
                    tMPs[i].text = GameSingleton.Instance.languageDictionary.SearchAndTraduce(tMPs[i].text,
                        GameSingleton.Instance.gameSettings.language);
                }
            }
            
            Text[] texts = Resources.FindObjectsOfTypeAll<Text>();
            int textsCount = texts.Length;
            for (int i = 0; i < textsCount; i++) {
                if (!notTranslated.Contains(texts[i])) {
                    texts[i].text = GameSingleton.Instance.languageDictionary.SearchAndTraduce(texts[i].text,
                        GameSingleton.Instance.gameSettings.language);
                }
            }
        }

        public void ChangeLanguage(int value) {
            Language oldLanguage = GameSingleton.Instance.gameSettings.language;
            Language newLanguage = (Language) value;
            if (newLanguage == oldLanguage) return;
            GameSingleton.Instance.gameSettings.language = newLanguage;
            TranslateView();
        }
    }
}