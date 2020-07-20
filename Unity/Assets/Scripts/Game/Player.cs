using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Game
{
    public class Player
    {
        public int gold;
        public int arcadeGold;
        public int currentLevel;
        public int currentLevelArcade;
        public Gamemode gamemode = Gamemode.NONE;
        public int currentScore = 0;
        public string token;
        public int goldStartLevel;
        public Inventory inventoryBackup;
        public int currentSeed;
        public DateTime beginGame;
        public Inventory storyModeInventory = ScriptableObject.CreateInstance<Inventory>();
        public Inventory arcadeModeInventory = ScriptableObject.CreateInstance<Inventory>();

        public Player()
        {
            Load();
        }

        public void Load()
        {
            gold = PlayerPrefs.GetInt("PlayerGold", 20);
            arcadeGold = PlayerPrefs.GetInt("arcadeGold", 150);
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            currentLevelArcade = PlayerPrefs.GetInt("CurrentLevelArcade", 0);
            goldStartLevel = gold;
            inventoryBackup = null;
            token = PlayerPrefs.GetString("connection.token", "");
        }

        public void Save()
        {
            PlayerPrefs.SetInt("PlayerGold", gold);
            PlayerPrefs.SetInt("arcadeGold", arcadeGold);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.SetInt("CurrentLevelArcade", currentLevelArcade);
            PlayerPrefs.SetString("connection.token", token);
            PlayerPrefs.Save();
        }

        public void BackupInventory(Inventory inventory)
        {
            inventoryBackup = new Inventory();
            inventoryBackup.consumables = new List<Consumable>(inventory.consumables);
            inventoryBackup.equipments = new List<Equipment>(inventory.equipments);
            inventoryBackup.units = new List<StoreUnit>(inventory.units);
        }

        public enum Gamemode
        {
            NONE,
            LEVEL,
            ARCADE,
            PERSONNALIZED
        }
    }
}