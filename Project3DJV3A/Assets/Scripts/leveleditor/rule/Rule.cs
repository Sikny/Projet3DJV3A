using System;
using System.Collections.Generic;
using System.IO;
using LevelEditor.Rule.Math;
using UnityEngine;

namespace LevelEditor.Rule {
    public class Rule
    {
        public byte size;
        // This following is set from .lvl file or randomize based on seedWorl in free-mode
        public int maxBudget;
        private char _globalDifficulty;
    
        public Dictionary<SerializedVector2, float> mapModifierHeightmap;
        public Dictionary<SerializedVector2, byte> localSpawnDifficulty; //SPEC : to avoid the gen into the castle

        //TODO
        public Rule()
        {
            mapModifierHeightmap = new Dictionary<SerializedVector2, float>();
            localSpawnDifficulty = new Dictionary<SerializedVector2, byte>();
        }

        public Rule(Vector3[] heightmap, Color[] difficulty, byte size, int money) : this()
        {
            this.size = size;
            maxBudget = money;
            float accurancyEpsilon = 0.1f;
            for (int i = 0; i < heightmap.Length; i++)
            {
            
                if (!(-accurancyEpsilon <= heightmap[i].y && heightmap[i].y <= accurancyEpsilon ))
                {
                    mapModifierHeightmap.Add(new SerializedVector2(heightmap[i].x, heightmap[i].z), heightmap[i].y);
                }
                if (accurancyEpsilon <= difficulty[i].r && -0.5f < heightmap[i].y && heightmap[i].y < 0.5f)
                {
                    localSpawnDifficulty.Add(new SerializedVector2(heightmap[i].x,heightmap[i].z),(byte)(difficulty[i].r*4) );
                }
            }
        }

        public Vector3[] LoadHeightmap(Vector3[] heightmap)
        {

            foreach (var entry in mapModifierHeightmap)
            {
                try
                {
                    heightmap[(int) (entry.Key.X) * (size) + (int) entry.Key.Z] =
                        new Vector3(entry.Key.X, entry.Value, entry.Key.Z);
                
                }
                catch (Exception) {
                    // ignored
                }
            }

            return heightmap;
        }
    
        public Color[] LoadDifficulty(Color[] difficulty)
        {

            foreach (var entry in localSpawnDifficulty)
            {
                try
                {
                    difficulty[(int) (entry.Key.X) * (size) + (int) entry.Key.Z] =
                        new Color(entry.Value/4f,0f,0f);
                
                }
                catch (Exception) {
                    // ignored
                }
            }

            return difficulty;
        }
    

        // CALMEZ VOUS
        public static void SaveLevel(string file, Rule r)
        {
            using (Stream stream = File.Open(Application.persistentDataPath+"/" +file+".lvl", FileMode.Create))
            {
                var bw = new BinaryWriter(stream);

                var sizeModifier = 12 * r.mapModifierHeightmap.Count;
                var sizeLocalDifficulty = 5 * r.localSpawnDifficulty.Count;

                var ptrModifier = 28; //
                var ptrLocalDifficulty = ptrModifier + sizeModifier;

                sizeModifier += ptrModifier;
                sizeLocalDifficulty += ptrLocalDifficulty;
            
            
                bw.Write((byte)r._globalDifficulty);
                bw.Write((short)r.maxBudget);
                bw.Write(r.size);
            
                bw.Write(ptrModifier);
                bw.Write(sizeModifier);
            
                bw.Write(ptrLocalDifficulty);
                bw.Write(sizeLocalDifficulty);

                //Debug.Log($"write {r.mapModifierHeightmap.Count*12}");
                //Debug.Log($"write {r.localSpawnDifficulty.Count*5}");
            
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
                    bw.Write(difficulty.Value);
                }
            
                bw.Close();
                stream.Close();
            
            }
        
        }
        public static Rule ReadLevel(string file)
        {
            using (Stream stream = File.Open(Application.persistentDataPath +"/"+ file + ".lvl", FileMode.Open))
            {
                var br = new BinaryReader(stream);
                Rule r = new Rule();

                var i = 0;
            
                r._globalDifficulty = (char) br.ReadByte();
                ++i;
                r.maxBudget = br.ReadInt16();
                i += 2;
                r.size = br.ReadByte();
                ++i;
            
                var unusedPtrModifier = br.ReadInt32();
                var sizeModifier = br.ReadInt32();
                var ptrLocalDifficulty = br.ReadInt32();
                var sizeLocalDifficulty = br.ReadInt32();
                i += 24;

                for (; i+12 <= sizeModifier; i += 12)
                {
                    var xCoord = br.ReadInt16();
                    var zCoord = br.ReadInt16();
                    var heightmap = br.ReadDouble();
                    r.mapModifierHeightmap.Add(new SerializedVector2(xCoord, zCoord), (float) heightmap);
                }

                for (; i+5 <= sizeLocalDifficulty; i += 5)
                {
                    var xCoord = br.ReadInt16();
                    var zCoord = br.ReadInt16();
                    var difficulty = br.ReadByte();
                    r.localSpawnDifficulty.Add(new SerializedVector2(xCoord, zCoord), difficulty);
                }
            
                //Debug.Log($"read {nbWrite}");
                //Debug.Log($"read {nbWrite2}");

                br.Close();
                stream.Close();
            
                return r;
            }
        }
    }
}
