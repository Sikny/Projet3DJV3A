using System;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        public LevelList levelList;

        public Level loadedLevel;

        public GenRandomParam grp;

        private int _levelCountArcade = 0; 
        private void Start()
        {
            if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE && GameSingleton.Instance.GetPlayer().currentScore == 0)
            {
                
                levelList.ClearLevels();
                GameSingleton.Instance.GetPlayer().currentLevelArcade = 0;
                _levelCountArcade = 0;
                GenerateLevel();
  
            }
            else if(GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE)
            {
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevelArcade));
                loadedLevel.Init();
            }
            else
            {
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevel));
                loadedLevel.Init();
            }
        }

        public void NextLevel() {
            if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE)
            {
                GameSingleton.Instance.GetPlayer().currentLevelArcade = 
                    (GameSingleton.Instance.GetPlayer().currentLevelArcade + 1) % levelList.LevelCount;
            }
            else
            {
                GameSingleton.Instance.GetPlayer().currentLevel = 
                    (GameSingleton.Instance.GetPlayer().currentLevel + 1) % levelList.LevelCount;
            }
        }

        public void SetLoadedLevel(Level level)
        {
            loadedLevel = level;
        }

        public void GenerateLevel()
        {
            _levelCountArcade++;
            loadedLevel = grp.generateNextLevel(Random.Range(Int32.MinValue, Int32.MaxValue),  _levelCountArcade+50);
            grp.setDefaultGold(loadedLevel);
            GameSingleton.Instance.levelManager = this;
            loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevelArcade));
            loadedLevel.Init();
            
            GameSingleton.Instance.GetPlayer().currentLevelArcade += 1;

            //generate
            //call when pressing free mode button in menu
            //call at end of level in arcade mode 
        }
    }
}
