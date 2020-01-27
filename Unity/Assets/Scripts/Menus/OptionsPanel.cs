using Language;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Menus {
    public class OptionsPanel : MonoBehaviour {
        public Slider soundsSlider;
        
        public Slider musicSlider;
        
        public Dropdown languageDropdown;

        private GameSettings _gameSettings;
        private LanguageDictionary _langDict;

        private void Awake() {
            if(_gameSettings == null) _gameSettings = Resources.Load<GameSettings>("Data/GameSettings");
            if (_langDict == null) _langDict = Resources.Load<LanguageDictionary>("Data/LanguagesDictionary");
            soundsSlider.value = _gameSettings.soundVolume;
            musicSlider.value = _gameSettings.musicVolume;
            
            Text[] texts = FindObjectsOfType<Text>();
            int textsCount = texts.Length;
            for (int i = 0; i < textsCount; i++) {
                texts[i].text = _langDict.GetString(texts[i].text, _gameSettings.language);
            }
        }
    }
}