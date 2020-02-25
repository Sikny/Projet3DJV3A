using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Language {
    public class Traducer : MonoBehaviour {
        public List<Text> notTranslated;

        private void Awake() {
            TranslateView();
        }

        public void TranslateView() {
            Text[] texts = Resources.FindObjectsOfTypeAll<Text>();
            int textsCount = texts.Length;
            for (int i = 0; i < textsCount; i++) {
                if (!notTranslated.Contains(texts[i])) {
                    texts[i].text = GameSingleton.Instance.languageDictionary.GetString(texts[i].text, 
                        GameSingleton.Instance.gameSettings.language);
                }
            }
        }
    }
}
