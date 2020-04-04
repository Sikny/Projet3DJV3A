using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Helmet", menuName = "ScriptableObjects/Equipments/Helmet")] 
public class Helmet : Equipment
{

   public int defenseStat = 5; //exemple 
   public override void Use()
   {
      base.Use();
      //écrit ton code ici 
   }
}
