using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Utility {
    [CreateAssetMenu(fileName = "LevelList", menuName = "ScriptableObjects/LevelList")]
    public class LevelList : ScriptableObject {
        [SerializeField] private List<Level> levels;

        public Level GetLevel(int index) {
            return levels[index];
        }
    }
}
