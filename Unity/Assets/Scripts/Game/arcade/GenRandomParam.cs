using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using TerrainGeneration;
using UnityEngine;
using Utility;
using Random = System.Random;

public class GenRandomParam : MonoBehaviour
{
    public LevelList levelList;
    public Level levelBase;

    public void Start()
    {
        
    }

    public void generateNextLevel(int seed, int i)
    {
        Random rand = new Random(seed);

        Level levelNew = Level.Instantiate(levelBase);

        TerrainOptions options = levelNew.terrainOptions;

        options.rules.seedWorld = seed;
        options.mountainCount = rand.Next(2, 8);
        options.waterCount = rand.Next(1, 3);
        options.maxWaterSize = rand.Next(10, 30);

        levelList.addLevel(levelNew);
    } 
}
