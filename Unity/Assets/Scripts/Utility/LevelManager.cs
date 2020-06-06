﻿using Game;
using UnityEngine;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        public LevelList levelList;

        public Level loadedLevel;

        public GenRandomParam grp;
        
        private void Awake()
        {
            grp.generateNextLevel(1024,0);
            
            GameSingleton.Instance.levelManager = this;
            loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevel));
            loadedLevel.Init();
        }

        public void NextLevel() {
            GameSingleton.Instance.GetPlayer().currentLevel = 
                (GameSingleton.Instance.GetPlayer().currentLevel + 1) % levelList.LevelCount;
        }
    }
}
