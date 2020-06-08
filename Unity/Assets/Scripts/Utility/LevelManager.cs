using System;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        public LevelList levelList;

        public Level loadedLevel;

        public GenRandomParam grp;

        private int _levelCount = 0; 
        private void Start()
        {
            if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE && GameSingleton.Instance.GetPlayer().currentScore == 0)
            {
                
                levelList.ClearLevels();
                GameSingleton.Instance.GetPlayer().currentLevel = 0;
                GenerateLevel();
  
            }
            else
            {
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevel));
                loadedLevel.Init();
            }
        }

        public void NextLevel() {
            GameSingleton.Instance.GetPlayer().currentLevel = 
                (GameSingleton.Instance.GetPlayer().currentLevel + 1) % levelList.LevelCount;
        }

        public void SetLoadedLevel(Level level)
        {
            loadedLevel = level;
        }

        public void GenerateLevel()
        {
            _levelCount++;
            loadedLevel = grp.generateNextLevel(Random.Range(Int32.MinValue, Int32.MaxValue), _levelCount);
            grp.setDefaultGold(loadedLevel);
            GameSingleton.Instance.levelManager = this;
            Debug.Log("player current level:" + GameSingleton.Instance.GetPlayer().currentLevel);
            loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevel));
            loadedLevel.Init();
            //generate
            //call when pressing free mode button in menu
            //call at end of level in arcade mode 
        }
    }
}
