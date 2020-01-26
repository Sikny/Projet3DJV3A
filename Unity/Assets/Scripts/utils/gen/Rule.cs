using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule
{
    const int MAX_BUDGET = 2000;
    const int MIN_BUDGET = 1000;
    const int ENNEMY_SPAWN_RADIUS = 100; // Temporary


    int seedWorld; // used in free-mode(0 in level mode)

    // This following is set from .lvl file or randomize based on seedWorl in free-mode
    int maxBudget;
    int globalDifficulty;

    Dictionary<Vector2, float> mapModifierHeightmap;
    Dictionary<Vector2, int> localSpawnDifficulty; //SPEC : to avoid the gen into the castle
    Dictionary<Vector3, int> mapCastlePiecesPlacement; 

    //TODO
    public Rule()
    {

    }
    // Free-mode
    public Rule(string seedUser)
    {
        if (!string.IsNullOrWhiteSpace(seedUser))
        {
            if (!int.TryParse(seedUser, out this.seedWorld))
            {
                this.seedWorld = seedUser.GetHashCode();
            }
        }
        else
        {
            System.Random rand = new System.Random();
            this.seedWorld = rand.Next();
        }

        System.Random genRand = new System.Random(this.seedWorld);

        this.maxBudget = genRand.Next(MIN_BUDGET, MAX_BUDGET);
        this.globalDifficulty = 2;

        // Def authorized spawn
        
        for(float i = 0f; i <= Mathf.PI * 2; i += 1/(Mathf.PI * ENNEMY_SPAWN_RADIUS))
        {
            int xCoor = (int)(ENNEMY_SPAWN_RADIUS * Mathf.Cos(i));
            int zCoor = (int)(ENNEMY_SPAWN_RADIUS * Mathf.Sin(i));

            Vector2 posSpawner = new Vector2(xCoor, zCoor);

            if(genRand.Next(0,5) == 0)
            {
                this.localSpawnDifficulty.Add(posSpawner, genRand.Next(1, 5));
            }

        }
    }
}
