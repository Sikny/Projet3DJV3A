using System;
using UnityEngine;

namespace Game {
    public class Player {
        public int gold;
        public int arcadeGold;
        public int currentLevel;
        public int currentLevelArcade;
        public Gamemode gamemode = Gamemode.LEVEL;
        public int currentScore = 0;
        public string token;
        public int goldStartLevel;
        public Inventory inventoryStartLevel;
        public int currentSeed;
        public DateTime beginGame;
        public Inventory storyModeInventory = ScriptableObject.CreateInstance<Inventory>();
        public Inventory arcadeModeInventory  = ScriptableObject.CreateInstance<Inventory>();
        public Player() {
            Load();
        }

        public void Load() {
            gold = PlayerPrefs.GetInt("PlayerGold", 100);
            arcadeGold = PlayerPrefs.GetInt("arcadeGold", 150);
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            currentLevelArcade = PlayerPrefs.GetInt("CurrentLevelArcade", 0);
            goldStartLevel = gold;
            inventoryStartLevel = storyModeInventory;
            token = PlayerPrefs.GetString("connection.token", "");
        }

        public void Save() {
            PlayerPrefs.SetInt("PlayerGold", gold);
            PlayerPrefs.SetInt("arcadeGold", arcadeGold);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.SetInt("CurrentLevelArcade", currentLevelArcade);
            PlayerPrefs.SetString("connection.token", token);
            PlayerPrefs.Save();
        }
        
        public enum Gamemode
        {
            LEVEL,
            ARCADE,
            PERSONNALIZED
        }
    }
    
}