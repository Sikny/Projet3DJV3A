using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Terrain;
using Units;
using UnityEngine;
using Utility;
using Random = System.Random;
using rand = UnityEngine.Random;


public class GenRandomParam : MonoBehaviour
{
    public LevelList levelList;
    public Level levelBase;

    
    public Level generateNextLevel(int seed, int i)
    {
        //int difficulty;
        seed = seed + (seed+1) * i;
        
        Random rand = new Random(seed);

        Level levelNew = levelBase;//Instantiate(levelBase);

        TerrainOptions options = levelNew.terrainBuilder.terrainOptions;

        options.rules.seedWorld = seed;
        options.mountainCount = rand.Next(2, 8);
        options.waterCount = rand.Next(1, 3);
        options.maxWaterSize = rand.Next(10, 30);

        int nbEnnemy =  rand.Next(1, i) + rand.Next( (int)(i/4f), (int)(i/2f));
        nbEnnemy = nbEnnemy > 8 ? 8 : nbEnnemy;

        for (int j = 0; j < nbEnnemy; j++)
        {
            EnemySpawn es = new EnemySpawn();
            
            es.position = new Vector2(rand.Next(-25,25), rand.Next(-25,25));
            
            

            Array values = Enum.GetValues(typeof(EntityType));
            es.entityType = (EntityType)values.GetValue(rand.Next(values.Length));
            levelNew.enemySpawns.Add(es);
        }
        levelList.addLevel(levelNew);
        return levelNew;
    }

    public void setDefaultGold(Level l)
    {
        
        GameSingleton.Instance.GetPlayer().gold = l.enemySpawns.Count * 75;
    }
}
