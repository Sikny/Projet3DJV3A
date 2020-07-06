using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terrain {
    [Serializable]
    public class MaterialGradientPair {
        public Material material;
        public Gradient gradient;
    }
    [CreateAssetMenu(fileName = "GradientList", menuName = "ScriptableObject/GradientList")]
    public class GradientList : ScriptableObject {
        public List<MaterialGradientPair> gradientsList = new List<MaterialGradientPair>();

        public MaterialGradientPair GetRandomGradient() {
            int ind = Random.Range(0, gradientsList.Count);
            return gradientsList[ind];
        }
    }
}
