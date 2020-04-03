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
   }

   public int GetGold()
   {
      return gold;
   }



}
