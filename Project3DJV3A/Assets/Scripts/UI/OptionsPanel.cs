using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI {
    public class OptionsPanel : MonoBehaviour {
        public Slider soundsSlider;
        
        public Slider musicSlider;
        
        public Dropdown languageDropdown;

        public bool invertCameraX;
        public bool invertCameraY;
        
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
        
        public void ResetSave()
        {
            PlayerPrefs.DeleteAll();
            Popups.instance.PopupTop("Save has been reset! Restart game to take effect.");
        }

        public void SetInvertCameraX()
        {
            invertCameraX = !invertCameraX;
        }        
        
        public void SetInvertCameraY()
        {
            invertCameraY = !invertCameraY;
        }
        public void OnSubmit()
        {
            GameSingleton.Instance.gameSettings.soundVolume = soundsSlider.value;
            GameSingleton.Instance.gameSettings.musicVolume = musicSlider.value;
            GameSingleton.Instance.gameSettings.invertCameraX = invertCameraX;
            GameSingleton.Instance.gameSettings.invertCameraY = invertCameraY;
        }

        public void UpdateSoundValue() {
            GameSingleton.Instance.gameSettings.soundVolume = soundsSlider.value;
            GameSingleton.Instance.soundManager.UpdateVolume();
        }

        public void UpdateMusicValue() {
            GameSingleton.Instance.gameSettings.musicVolume = musicSlider.value;
            GameSingleton.Instance.soundManager.UpdateVolume();
        }
    }
    

}