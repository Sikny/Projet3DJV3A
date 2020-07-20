using System;
using System.Linq;
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

            options.width = options.height = rand.Next(20, 40);
            options.seed = seed;
            options.mountainCount = rand.Next(2, 8);
            options.waterCount = rand.Next(1, 5);
            options.maxWaterSize = rand.Next(10, 40);

            levelNew.enemySpawns.Clear();
            Debug.Log("i="+i);
            int nbEnnemy =  rand.Next(i, i*2);
            nbEnnemy = 8;//nbEnnemy > 8 ? 8 : nbEnnemy;

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

        public Level RespawnEnnemies(Level l)
        {
            Random rand = new Random(GameSingleton.Instance.GetPlayer().currentSeed);
            var height = l.terrainBuilder.terrainOptions.modifierHeightMap;
            var epsilon = 0.1f;
            var heightNotGround = height.Where(x => x.Value < -epsilon || epsilon < x.Value).Select(x => x.Key).ToList();
            foreach (var ennemy in l.enemySpawns)
            {
                bool noHeightNear = true;
                
                while (heightNotGround.Contains(ennemy.position) && noHeightNear)
                {
                    
                    ennemy.position = new Vector2(rand.Next(-l.rule.size/2+1,l.rule.size/2-1), rand.Next(-l.rule.size/2+1,l.rule.size/2-1));
                    
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
    
        public void setDefaultGold(Level l)
        {
        
            //GameSingleton.Instance.GetPlayer().gold = l.enemySpawns.Count * 100;
        }
    }
}
