using System;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration {
    public enum ZoneType {
        Dirt,
        Grass,
        Sand,
        Water,
        Count    // can't be used, must stay at last
    }

    [Serializable]
    public class ZoneMaterial {
        public ZoneType type;
        public TerrainUnit unitPrefab;
    }

    [CreateAssetMenu(fileName = "UnitDict", menuName = "ScriptableObject/UnitDict")]
    public class UnitDict : ScriptableObject {
        public List<ZoneMaterial> unitZoneMaterials;

        public TerrainUnit GetPrefab(ZoneType type) {
            for (int i = 0; i < unitZoneMaterials.Count; i++) {
                if (unitZoneMaterials[i].type == type)
                    return unitZoneMaterials[i].unitPrefab;
            }
            return null;
        }
    }
}