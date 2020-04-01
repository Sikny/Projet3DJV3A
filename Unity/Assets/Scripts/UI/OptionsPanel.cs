using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI {
    public class OptionsPanel : MonoBehaviour {
        public Slider soundsSlider;
        
        public Slider musicSlider;
        
        public Dropdown languageDropdown;

        private void Awake() {
            soundsSlider.value = GameSingleton.Instance.gameSettings.soundVolume;
            musicSlider.value = GameSingleton.Instance.gameSettings.musicVolume;

            int value = 0;
            foreach (Language.Language lang in Enum.GetValues(typeof(Language.Language))) {
                languageDropdown.options.Add(new Dropdown.OptionData(lang.ToString()));
                if (lang == GameSingleton.Instance.gameSettings.language) {
                    languageDropdown.value = value;
                }
                value++;
            }
        }
    }
}