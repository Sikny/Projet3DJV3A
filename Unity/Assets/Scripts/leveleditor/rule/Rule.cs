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
    public byte size;
    // This following is set from .lvl file or randomize based on seedWorl in free-mode
    public int maxBudget;
    char globalDifficulty;
    
    public Dictionary<SeriaVector2, float> mapModifierHeightmap;
    public Dictionary<SeriaVector2, byte> localSpawnDifficulty; //SPEC : to avoid the gen into the castle
    Dictionary<int, Castle> mapCastlePiecesPlacement;

    //TODO
    public Rule()
    {
        mapModifierHeightmap = new Dictionary<SeriaVector2, float>();
        localSpawnDifficulty = new Dictionary<SeriaVector2, byte>();
        mapCastlePiecesPlacement = new Dictionary<int, Castle>();
    }

    public Rule(Vector3[] heightmap, Color[] difficulty, byte size, int money) : this()
    {
        this.size = size;
        this.maxBudget = money;
        float accurancyEpsilon = 0.1f;
        for (int i = 0; i < heightmap.Length; i++)
        {
            
            if (!(-accurancyEpsilon <= heightmap[i].y && heightmap[i].y <= accurancyEpsilon ))
            {
                mapModifierHeightmap.Add(new SeriaVector2(heightmap[i].x, heightmap[i].z), heightmap[i].y);
            }
            if (accurancyEpsilon <= difficulty[i].r && -0.5f < heightmap[i].y && heightmap[i].y < 0.5f)
            {
                localSpawnDifficulty.Add(new SeriaVector2(heightmap[i].x,heightmap[i].z),(byte)(difficulty[i].r*4) );
            }
        }
    }

    public Vector3[] loadHeightmap(Vector3[] heightmap)
    {

        foreach (var entry in mapModifierHeightmap)
        {
            try
            {
                heightmap[(int) (entry.Key.X) * (size) + (int) entry.Key.Z] =
                    new Vector3(entry.Key.X, entry.Value, entry.Key.Z);
                
            }
            catch (Exception ex)
            {
                Debug.Log("problem");
            }
        }

        return heightmap;
    }
    
    public Color[] loadDifficulty(Color[] difficulty)
    {

        foreach (var entry in localSpawnDifficulty)
        {
            try
            {
                difficulty[(int) (entry.Key.X) * (size) + (int) entry.Key.Z] =
                    new Color(entry.Value/4f,0f,0f);
                Debug.Log("ok");
                
            }
            catch (Exception ex)
            {
                Debug.Log("problem");
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
        using (Stream stream = File.Open(Application.persistentDataPath+"/" +file+".lvl", FileMode.Create))
        {
            var bw = new BinaryWriter(stream);

            var sizeModifier = 12 * r.mapModifierHeightmap.Count;
            var sizeLocalDifficulty = 5 * r.localSpawnDifficulty.Count;
            var sizeCastle = 7 * r.mapCastlePiecesPlacement.Count;

            var ptrModifier = 28; //
            var ptrLocalDifficulty = ptrModifier + sizeModifier;
            var ptrCastle = ptrLocalDifficulty + sizeLocalDifficulty;

            sizeModifier += ptrModifier;
            sizeLocalDifficulty += ptrLocalDifficulty;
            
            
            bw.Write((byte)r.globalDifficulty);
            bw.Write((short)r.maxBudget);
            bw.Write((byte)r.size);
            
            bw.Write((int)ptrModifier);
            bw.Write((int)sizeModifier);
            
            bw.Write((int)ptrLocalDifficulty);
            bw.Write((int)sizeLocalDifficulty);
            
            bw.Write((int)ptrCastle);
            bw.Write((int)sizeCastle);
            
            Debug.Log($"write {r.mapModifierHeightmap.Count*12}");
            Debug.Log($"write {r.localSpawnDifficulty.Count*5}");
            
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
                bw.Write((byte)difficulty.Value);
            }
            
            bw.Close();
            stream.Close();
            
        }
        
    }
    public static Rule readLevel(string file)
    {
        using (Stream stream = File.Open(Application.persistentDataPath +"/"+ file + ".lvl", FileMode.Open))
        {
            var br = new BinaryReader(stream);
            Rule r = new Rule();

            var i = 0;
            
            r.globalDifficulty = (char) br.ReadByte();
            i += 1;
            r.maxBudget = br.ReadInt16();
            i += 2;
            r.size = br.ReadByte();
            i += 1;
            
            var ptrModifier = br.ReadInt32();
            var sizeModifier = br.ReadInt32();
            var ptrLocalDifficulty = br.ReadInt32();
            var sizeLocalDifficulty = br.ReadInt32();
            var ptrCastle = br.ReadInt32();
            var sizeCastle = br.ReadInt32();
            i += 24;

            var nbWrite = 0;
            var nbWrite2 = 0;

            for (; i+12 <= sizeModifier; i += 12)
            {
                var xCoord = br.ReadInt16();
                var zCoord = br.ReadInt16();
                var heightmap = br.ReadDouble();
                nbWrite+= 12;
                r.mapModifierHeightmap.Add(new SeriaVector2(xCoord, zCoord), (float) heightmap);
            }

            for (; i+5 <= sizeLocalDifficulty; i += 5)
            {
                var xCoord = br.ReadInt16();
                var zCoord = br.ReadInt16();
                var difficulty = br.ReadByte();
                nbWrite2 += 5;
                r.localSpawnDifficulty.Add(new SeriaVector2(xCoord, zCoord), difficulty);
            }
            
            Debug.Log($"read {nbWrite}");
            Debug.Log($"read {nbWrite2}");

            br.Close();
            stream.Close();
            
            return r;
        }
    }
}
