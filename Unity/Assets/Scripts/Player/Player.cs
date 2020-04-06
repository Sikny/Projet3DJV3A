using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public int gold = 0;
   //private List<Units> units;
   //private Inventory inventory;
   //public List<Item> items;
   //public List<Item> equipments;
   //private List<Units> units; 
   
   
   public static Player instance;

   private void Awake()
   {
      if (instance != null)
      {
         Debug.Log("Several instances");
         return;
      }
      instance = this;

      Load();
   }

   public void Load() {
      gold = PlayerPrefs.GetInt("PlayerGold", 0);
   }

   public int GetGold()
   {
      return gold;
   }

   public void Save() {
      PlayerPrefs.SetInt("PlayerGold", gold);
      PlayerPrefs.Save();
   }
}
