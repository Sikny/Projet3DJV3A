using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Language {
    public class Traducer : MonoBehaviour {
        public List<Text> notTranslated;
        
        private Language _currentLang;
        private LanguageDictionary _langDict;

        private void Awake() {
            if (_langDict == null) _langDict = Resources.Load<LanguageDictionary>("Data/LanguagesDictionary");
            _currentLang = Resources.Load<GameSettings>("Data/GameSettings").language;
            
            TranslateView();
        }

        public void TranslateView() {
            Text[] texts = Resources.FindObjectsOfTypeAll<Text>();
            int textsCount = texts.Length;
            for (int i = 0; i < textsCount; i++) {
                if (!notTranslated.Contains(texts[i]))
                    texts[i].text = _langDict.GetString(texts[i].text, _currentLang);
            }
        }
    }
}
