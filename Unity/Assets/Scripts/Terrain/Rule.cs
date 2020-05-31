using System;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain {
    [Serializable]
    public class Rule {
        const int EnnemySpawnRadius = 100; // Temporary
        
        public int seedWorld;    // used in free-mode(0 in level mode)

        Dictionary<Vector2, int> localSpawnDifficulty; //SPEC : to avoid the gen into the castle

        //TODO
        public Rule() {
            localSpawnDifficulty = new Dictionary<Vector2, int>();
        }
        // Free-mode
        public Rule(string seedUser) : this(){
            if (!string.IsNullOrEmpty(seedUser)) {
                if (!int.TryParse(seedUser, out this.seedWorld)) {
                    seedWorld = seedUser.GetHashCode();
                }
            } else {
                System.Random rand = new System.Random();
                seedWorld = rand.Next();
            }
            System.Random genRand = new System.Random(seedWorld);

            // Def authorized spawn
        
            for(float i = 0f; i <= Mathf.PI * 2; i += 1/(Mathf.PI * EnnemySpawnRadius)) {
                int xCoor = (int)(EnnemySpawnRadius * Mathf.Cos(i));
                int zCoor = (int)(EnnemySpawnRadius * Mathf.Sin(i));

                Vector2 posSpawner = new Vector2(xCoor, zCoor);

                if(genRand.Next(0,5) == 0) {
                    // 5 levels of localdifficulty (0 = no spawn; 5 = max spawn)
                    localSpawnDifficulty.Add(posSpawner, genRand.Next(1, 5));
                }
            }
        }
    }
}
