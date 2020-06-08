﻿using UnityEngine;
using UnityEngine.WSA;

namespace Game {
    public class Player {
        public int gold;
        public int currentLevel;
        public int currentLevelArcade;
        public Gamemode gamemode = Gamemode.LEVEL;
        public int currentScore = 0;
        public int goldStartLevel; 
        
        public Player() {
            Load();
        }

        public void Load() {
            gold = PlayerPrefs.GetInt("PlayerGold", 50);
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            currentLevelArcade = PlayerPrefs.GetInt("CurrentLevelArcade", 0);
            goldStartLevel = gold;
        }

        public int GetGold() {
            return gold;
        }

        public void Save() {
            PlayerPrefs.SetInt("PlayerGold", gold);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.SetInt("CurrentLevelArcade", currentLevelArcade);
            PlayerPrefs.Save();
        }
        
        public enum Gamemode
        {
            LEVEL,
            ARCADE
        }
    }
    
}