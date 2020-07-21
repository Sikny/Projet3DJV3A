using System;
using Terrain;
using Units;
using UnityEngine;
using Utility;
using Random = System.Random;


namespace Game.arcade {
    public class GenRandomParam : MonoBehaviour
    {
        public LevelList levelList;
        public Level levelBase;

    
        public Level GenerateNextLevel(int seed, int i)
        {
            //int difficulty;
            seed = seed + (seed+1) * i;
            Random rand = new Random(seed);

            Level levelNew = levelBase;//Instantiate(levelBase);

            TerrainOptions options = levelNew.terrainBuilder.terrainOptions;

            int[] sizes = {20, 30, 40};
            var randInd = rand.Next(0, sizes.Length);
            options.width = options.height = sizes[randInd];
            options.seed = seed;
            options.mountainCount = rand.Next(2, 6);
            options.waterCount = rand.Next(1, 5);
            options.maxWaterSize = rand.Next(10, 40);

            levelNew.enemySpawns.Clear();
            int nbEnnemy =  rand.Next(i, i*2);
            //nbEnnemy = 8;//nbEnnemy > 8 ? 8 : nbEnnemy;

            for (int j = 0; j < nbEnnemy; j++)
            {
                EnemySpawn es = new EnemySpawn();

                es.position = new Vector2(rand.Next(-options.width/2+1,options.width/2-1), rand.Next(-options.height/2+1,options.height/2-1));
                
                Array values = Enum.GetValues(typeof(EntityType));
                es.entityType = (EntityType)values.GetValue(rand.Next(values.Length));
                es.entityType = SoftEntityType(rand, es.entityType, (i - 2)/3f );
                levelNew.enemySpawns.Add(es);
            }
            levelList.addLevel(levelNew);
            return levelNew;
        }

        public static EntityType SoftEntityType(Random rand, EntityType type, float difficult)
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
    }
}
