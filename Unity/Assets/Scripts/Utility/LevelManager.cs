using System;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility {
    public class LevelManager : MonoBehaviour {
        public LevelList levelList;

        public Level loadedLevel;

        public GenRandomParam grp;
        
        private void Start()
        {
            if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE)
            {
                Debug.Log("CALLED");
                GameSingleton.Instance.levelManager = this;
               /* loadedLevel = grp.generateNextLevel(Random.Range(Int32.MinValue, Int32.MaxValue), 5);
                grp.setDefaultGold(loadedLevel);
                GameSingleton.Instance.levelManager = this;
                loadedLevel = Instantiate(levelList.GetLevel(GameSingleton.Instance.GetPlayer().currentLevel));
                loadedLevel.Init();*/
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
    }
}
