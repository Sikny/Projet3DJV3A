using Units;
using UnityEngine;

namespace Items.Equipments {
   [CreateAssetMenu(fileName = "Shield", menuName = "ScriptableObject/Equipments/Shield")] 
   public class Shield : global::Items.Equipment
   {
      public int level;
      public override void Use()
      {
         if (UnitLibData.selectedUnit != null)
         {
         
            UnitLibData.selectedUnit.AddEffect(1,level,float.PositiveInfinity);
         
            base.Use(); 
         }
         else
         {
         
         }//TODO gérer un message d'erreur
      }
   }
}
