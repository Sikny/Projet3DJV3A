using UnityEngine;

namespace Game {
    public class Player {
        public int gold;
        public int currentLevel;
        public Gamemode gamemode = Gamemode.LEVEL;
        
        public Player() {
            Load();
        }

        public void Load() {
            gold = PlayerPrefs.GetInt("PlayerGold", 50);
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        }

        public int GetGold() {
            return gold;
        }

        public void Save() {
            PlayerPrefs.SetInt("PlayerGold", gold);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.Save();
        }
        
        public enum Gamemode
        {
            LEVEL,
            ARCADE
        }
    }
    
}