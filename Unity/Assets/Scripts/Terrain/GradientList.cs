using System.Collections.Generic;
using UnityEngine;

namespace Terrain {
    [CreateAssetMenu(fileName = "GradientList", menuName = "ScriptableObject/GradientList")]
    public class GradientList : ScriptableObject {
        public List<Gradient> gradientsList = new List<Gradient>();
    }
}
