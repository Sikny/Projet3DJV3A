using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        options.mountainCount = rand.Next(2, 15);
        options.waterCount = rand.Next(1, 5);
        options.maxWaterSize = rand.Next(10, 50);

        int nbEnnemy =  rand.Next(1, i) + rand.Next( (int)(i/4f), (int)(i/2f));
        nbEnnemy = nbEnnemy > 8 ? 8 : nbEnnemy;

        for (int j = 0; j < nbEnnemy; j++)
        {
            EnemySpawn es = new EnemySpawn();
            
            //Vector2 spawnPosition = new Vector2(rand.Next(-25,25), rand.Next(-25,25));
            /*
             * Check heightmap at coordinate and surrounding, if the heightmap is 0 place unit, else reselect a spawn position
             */

            es.position = new Vector2(rand.Next(-25,25), rand.Next(-25,25));

            

            Array values = Enum.GetValues(typeof(EntityType));
            es.entityType = (EntityType)values.GetValue(rand.Next(values.Length));
            es.entityType = softEntityType(rand, es.entityType, (i - 2)/3f );
            levelNew.enemySpawns.Add(es);
        }
        levelList.addLevel(levelNew);
        return levelNew;
    }

    public Level respawnEnnemies(Level l)
    {
        Random rand = new Random(GameSingleton.Instance.GetPlayer().currentSeed);
        var height = l.terrainBuilder.terrainOptions.modifierHeightMap;
        var heightNotGround = height.Where(x => x.Value != 0).Select(x => x.Key).ToList();
        foreach (var ennemy in l.enemySpawns)
        {
            bool noHeightNear = true;

            while (heightNotGround.Contains(ennemy.position) && noHeightNear)
            {
                noHeightNear = true;
                ennemy.position = new Vector2(rand.Next(-24,24),rand.Next(-24,24));
                if (heightNotGround.Contains(ennemy.position))
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            Vector2 nextPos = new Vector2(x,y);
                            if (heightNotGround.Contains(nextPos))
                            {
                                noHeightNear = false;
                                //ennemy.position = nextPos;
                                break;
                            }
                        }
                    }
                }

            }
        }
        return l;
    }

    
    public static EntityType softEntityType(Random rand, EntityType type, float difficult)
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
