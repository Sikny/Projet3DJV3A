using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Rule
{
    const int MAX_BUDGET = 2000;
    const int MIN_BUDGET = 1000;
    const int ENNEMY_SPAWN_RADIUS = 100; // Temporary


    int seedWorld; // used in free-mode(0 in level mode)

    // This following is set from .lvl file or randomize based on seedWorl in free-mode
    int maxBudget;
    char globalDifficulty;
    
    Dictionary<SeriaVector2, float> mapModifierHeightmap;
    Dictionary<SeriaVector2, int> localSpawnDifficulty; //SPEC : to avoid the gen into the castle
    Dictionary<int, Castle> mapCastlePiecesPlacement;

    //TODO
    public Rule()
    {
        mapModifierHeightmap = new Dictionary<SeriaVector2, float>();
        localSpawnDifficulty = new Dictionary<SeriaVector2, int>();
        mapCastlePiecesPlacement = new Dictionary<int, Castle>();
    }

    public Rule(Vector3[] heightmap, Color[] difficulty) : this()
    {
        float accurancyEpsilon = 0.1f;
        for (int i = 0; i < heightmap.Length; i++)
        {
            
            if (!(-accurancyEpsilon <= heightmap[i].y && heightmap[i].y <= accurancyEpsilon ))
            {
                mapModifierHeightmap.Add(new SeriaVector2(heightmap[i].x, heightmap[i].z), heightmap[i].y);
            }
            if (accurancyEpsilon <= difficulty[i].r)
            {
                localSpawnDifficulty.Add(new SeriaVector2(heightmap[i].x,heightmap[i].z),(int)(difficulty[i].r*4) );
            }
        }
    }

    public Vector3[] loadHeightmap(Vector3[] heightmap, int size)
    {

        foreach (var entry in mapModifierHeightmap)
        {
            try
            {
                heightmap[(int) (entry.Key.X+size) * (size*2+1) + (int) entry.Key.Z+size] =
                    new Vector3(entry.Key.X, entry.Value, entry.Key.Z);
                
            }
            catch (Exception ex)
            {
                
            }
        }

        return heightmap;
    }
    
    public Color[] loadDifficulty(Color[] difficulty, int size)
    {

        foreach (var entry in localSpawnDifficulty)
        {
            try
            {
                difficulty[(int) (entry.Key.X+size) * (size*2+1) + (int) entry.Key.Z+size] =
                    new Color(entry.Value/4f,0f,0f);
                
            }
            catch (Exception ex)
            {
                
            }
        }

        return difficulty;
    }
    
    // Random gen
    /*
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
        this.localSpawnDifficulty = new Dictionary<SeriaVector2, int>();
        for(float i = 0f; i <= Mathf.PI * 2; i += Mathf.PI / ( ENNEMY_SPAWN_RADIUS))
        {
            int xCoor = (int)(ENNEMY_SPAWN_RADIUS * Mathf.Cos(i));
            int zCoor = (int)(ENNEMY_SPAWN_RADIUS * Mathf.Sin(i));

            SeriaVector2 posSpawner = new SeriaVector2(xCoor, zCoor);

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
                Debug.Log("Pos " + posSpawner + " : " + levelOfDifficulty);
            }

        }
    }*/

    // CALMEZ VOUS
    public static void saveLevel(string file, Rule r)
    {
        using (Stream stream = File.Open("levels/"+file+".lvl", FileMode.Create))
        {
            var bw = new BinaryWriter(stream);

            var sizeModifier = 12 * r.mapModifierHeightmap.Count;
            var sizeLocalDifficulty = 5 * r.localSpawnDifficulty.Count;
            var sizeCastle = 7 * r.mapCastlePiecesPlacement.Count;

            var ptrModifier = 27;
            var ptrLocalDifficulty = ptrModifier + sizeModifier;
            var ptrCastle = ptrLocalDifficulty + sizeLocalDifficulty;
            
            
            bw.Write(r.globalDifficulty);
            bw.Write((short)r.maxBudget);
            
            bw.Write((int)ptrModifier);
            bw.Write((int)sizeModifier);
            
            bw.Write((int)ptrLocalDifficulty);
            bw.Write((int)sizeLocalDifficulty);
            
            bw.Write((int)ptrCastle);
            bw.Write((int)sizeCastle);
            
            foreach (var vertex in r.mapModifierHeightmap)
            {
                bw.Write((short)vertex.Key.X);
                bw.Write((short)vertex.Key.Z);
                bw.Write((double)vertex.Value);
            }
            
            foreach (var difficulty in r.localSpawnDifficulty)
            {
                bw.Write((short)difficulty.Key.X);
                bw.Write((short)difficulty.Key.Z);
                bw.Write((char)difficulty.Value);
            }
            
            bw.Close();
            stream.Close();
            
        }
        
    }
    public static Rule readLevel(string file)
    {
        using (Stream stream = File.Open("levels/" + file + ".lvl", FileMode.Open))
        {
            var br = new BinaryReader(stream);
            Rule r = new Rule();

            var i = 0;
            
            r.globalDifficulty = br.ReadChar();
            i += 1;
            r.maxBudget = br.ReadInt16();
            i += 2;
            
            var ptrModifier = br.ReadInt32();
            var sizeModifier = br.ReadInt32();
            var ptrLocalDifficulty = br.ReadInt32();
            var sizeLocalDifficulty = br.ReadInt32();
            var ptrCastle = br.ReadInt32();
            var sizeCastle = br.ReadInt32();
            i += 24;

            for (; i < sizeModifier; i += 12)
            {
                var xCoord = br.ReadInt16();
                var zCoord = br.ReadInt16();
                var heightmap = br.ReadDouble();

                r.mapModifierHeightmap.Add(new SeriaVector2(xCoord, zCoord), (float) heightmap);
            }

            for (; i < sizeLocalDifficulty; i += 5)
            {
                var xCoord = br.ReadInt16();
                var zCoord = br.ReadInt16();
                var difficulty = br.ReadChar();

                r.localSpawnDifficulty.Add(new SeriaVector2(xCoord, zCoord), difficulty);
            }

            br.Close();
            stream.Close();
            
            return r;
        }
    }
}
