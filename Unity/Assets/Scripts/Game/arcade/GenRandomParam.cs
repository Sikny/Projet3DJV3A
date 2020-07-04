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

        options.seed = seed;
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
            es.entityType = softEntityType(rand, es.entityType, (i - 2)/3f );
            levelNew.enemySpawns.Add(es);
        }
        levelList.addLevel(levelNew);
        return levelNew;
    }

    public EntityType softEntityType(Random rand, EntityType type, float difficult)
    {
        if (rand.NextDouble() > difficult)
        {
            switch (type)
            {
                case EntityType.Arbalist: type = EntityType.Archer; break;
                case EntityType.Bard: type = EntityType.Mage; break;
                case EntityType.Catapultist: type = EntityType.Archer;break;
                case EntityType.Demonist: type = EntityType.Mage;break;
                case EntityType.Executionist: type = EntityType.Soldier;break;
                case EntityType.Horseman: type = EntityType.Soldier;break;
                case EntityType.Hunter: type = EntityType.Archer;break;
                case EntityType.Knight: type = EntityType.Soldier;break;
                case EntityType.Sniper: type = EntityType.Archer;break;
                case EntityType.Spearman: type = EntityType.Soldier;break;
                case EntityType.BlackMage: type = EntityType.Mage;break;
                case EntityType.MachineArc: type = EntityType.Archer;break;
                case EntityType.RedMage: type = EntityType.Mage;break;
                case EntityType.WhiteKnight: type = EntityType.Soldier;break;
                case EntityType.WhiteMage: type = EntityType.Mage;break;
            }
        }

        return type;
    }
    
    public void setDefaultGold(Level l)
    {
        
        //GameSingleton.Instance.GetPlayer().gold = l.enemySpawns.Count * 100;
    }
}
