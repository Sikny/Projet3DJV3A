using System;
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

        private void Awake() {
            if(_gameSettings == null) _gameSettings = Resources.Load<GameSettings>("Data/GameSettings");
            soundsSlider.value = _gameSettings.soundVolume;
            musicSlider.value = _gameSettings.musicVolume;
            
        }
    }
}