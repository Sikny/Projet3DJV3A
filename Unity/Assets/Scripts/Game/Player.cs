using System;
using UnityEngine;

namespace Game {
    public class Player {
        public int gold;
        public int currentLevel;
        public int currentLevelArcade;
        public Gamemode gamemode = Gamemode.LEVEL;
        public int currentScore = 0;
        public string token;
        public int goldStartLevel;
        public int currentSeed;
        public DateTime beginGame;
        
        public Player() {
            Load();
        }

        public void Load() {
            gold = PlayerPrefs.GetInt("PlayerGold", 100);
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            currentLevelArcade = PlayerPrefs.GetInt("CurrentLevelArcade", 0);
            goldStartLevel = gold;
            token = PlayerPrefs.GetString("connection.token", "");
        }

        public int GetGold() {
            return gold;
        }

        public void Save() {
            PlayerPrefs.SetInt("PlayerGold", gold);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.SetInt("CurrentLevelArcade", currentLevelArcade);
            PlayerPrefs.SetString("connection.token", token);
            PlayerPrefs.Save();
        }
        
        public enum Gamemode
        {
            LEVEL,
            ARCADE
        }
    }
    
}