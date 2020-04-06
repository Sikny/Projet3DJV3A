﻿using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;


[CreateAssetMenu(fileName = "Shield", menuName = "ScriptableObjects/Equipments/Shield")] 
public class Shield : Equipment
{
   public int level;
   public override void Use()
   {
      if (UnitLibData._selectedUnit != null)
      {
         
         UnitLibData._selectedUnit.addEffect(1,level,float.PositiveInfinity);
         
         base.Use(); 
      }
      else
      {
         
      }//TODO gérer un message d'erreur
   }
}
