using System;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration {
    [Serializable]
    public class Rule {
        const int MaxBudget = 2000;
        const int MinBudget = 1000;
        const int EnnemySpawnRadius = 100; // Temporary
        
        public int seedWorld;    // used in free-mode(0 in level mode)

        // This following is set from .lvl file or randomize based on seedWorl in free-mode
        int maxBudget;
        int globalDifficulty;

        public Dictionary<Vector2, float> mapModifierHeightMap;
        Dictionary<Vector2, int> localSpawnDifficulty; //SPEC : to avoid the gen into the castle
        Dictionary<Vector3, int> mapCastlePiecesPlacement; 

        //TODO
        public Rule() {
            mapModifierHeightMap = new Dictionary<Vector2, float>();
        }
        // Free-mode
        public Rule(string seedUser) : this(){
            if (!string.IsNullOrWhiteSpace(seedUser)) {
                if (!int.TryParse(seedUser, out this.seedWorld)) {
                    this.seedWorld = seedUser.GetHashCode();
                }
            } else {
                System.Random rand = new System.Random();
                this.seedWorld = rand.Next();
            }
            System.Random genRand = new System.Random(this.seedWorld);
            this.maxBudget = genRand.Next(MinBudget, MaxBudget);
            this.globalDifficulty = 2;

            // Def authorized spawn
        
            for(float i = 0f; i <= Mathf.PI * 2; i += 1/(Mathf.PI * EnnemySpawnRadius)) {
                int xCoor = (int)(EnnemySpawnRadius * Mathf.Cos(i));
                int zCoor = (int)(EnnemySpawnRadius * Mathf.Sin(i));

                Vector2 posSpawner = new Vector2(xCoor, zCoor);

                if(genRand.Next(0,5) == 0) {
                    // 5 levels of localdifficulty (0 = no spawn; 5 = max spawn)
                    this.localSpawnDifficulty.Add(posSpawner, genRand.Next(1, 5));
                }

            }
        }
    }
}
