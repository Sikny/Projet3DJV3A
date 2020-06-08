using UI;
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
            UnitLibData.selectedUnit.AddEffect(1,level, 1000);
            //UnitLibData.selectedUnit.AddEquipment(1,level, this);
         
            base.Use(); 
         }
         else
         {
            Debug.Log("no selected unit");
            Popups.instance.Popup("No Unit Selected!", Color.red);
         }//TODO gérer un message d'erreur
      }
   }
}
