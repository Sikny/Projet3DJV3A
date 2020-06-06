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
            levels.Add(l);
            return LevelCount - 1;
        }
    }
}
