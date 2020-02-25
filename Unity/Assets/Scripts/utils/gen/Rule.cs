﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Rule
{
    const int MAX_BUDGET = 2000;
    const int MIN_BUDGET = 1000;
    const int ENNEMY_SPAWN_RADIUS = 100; // Temporary


    int seedWorld; // used in free-mode(0 in level mode)

    // This following is set from .lvl file or randomize based on seedWorl in free-mode
    int maxBudget;
    int globalDifficulty;

    //TODO : Serialize Vector2 into a new class (heritage?)
    Dictionary<Vector2, float> mapModifierHeightmap;
    Dictionary<Vector2, int> localSpawnDifficulty; //SPEC : to avoid the gen into the castle
    Dictionary<Vector3, int> mapCastlePiecesPlacement; // TODO : make castle class (including pos, rotation, scaling, id...)

    //TODO
    public Rule()
    {

    }
    // Free-mode
    public Rule(string seedUser)
    {
        if (!string.IsNullOrEmpty(seedUser))
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
        this.localSpawnDifficulty = new Dictionary<Vector2, int>();
        for(float i = 0f; i <= Mathf.PI * 2; i += Mathf.PI / ( ENNEMY_SPAWN_RADIUS))
        {
            int xCoor = (int)(ENNEMY_SPAWN_RADIUS * Mathf.Cos(i));
            int zCoor = (int)(ENNEMY_SPAWN_RADIUS * Mathf.Sin(i));

            Vector2 posSpawner = new Vector2(xCoor, zCoor);

            if(genRand.Next(0,20) == 0)
            {
                // 5 levels of localdifficulty (0 = no spawn; 5 = max spawn)
                int levelOfDifficulty = genRand.Next(1, 5);
                try
                {
                    this.localSpawnDifficulty.Add(posSpawner, levelOfDifficulty);
                }
                catch (ArgumentException ex)
                {

                }
                Debug.Log("Pos " + posSpawner.ToString() + " : " + levelOfDifficulty);
            }

        }
    }

    [Obsolete]
    public static void saveLevel(string file, Rule r)
    {
        using (Stream stream = File.Open("levels/"+file+".lvl", FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, r);
        }
    }
    public static Rule readLevel(string file)
    {
        using (Stream stream = File.Open("levels/" + file + ".lvl", FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Rule r = (Rule)binaryFormatter.Deserialize(stream);
            return r;
        }
    }
}