using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Utility {
    [CreateAssetMenu(fileName = "LevelList", menuName = "ScriptableObject/LevelList")]
    public class LevelList : ScriptableObject {
        [SerializeField] private List<Level> levels = new List<Level>();

        public int LevelCount => levels.Count;

        public Level GetLevel(int index) {
            return levels[index];
        }

        public int addLevel(Level l)
        {
            
/*            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i] == null)
                {
                    levels[i] = l;
                    break;
                }
            }*/
            levels.Add(l);
            return LevelCount - 1;
        }

        public void ClearLevels()
        {
            levels.Clear();
        }
    }
}
