using Units;
using UnityEngine;

/*
 * sers a donner un chemin ou tu peux crée une nouvelle potion
 * filename = nom par défaut du fichier de l'objet
 * menuName = nom dans le menu quand tu crée
 *
 * pour crée un nouveau à partir de sa clique droit dans l'asset --> scriptable objec --> le nom que t'as mis dans menuName
 */
namespace Items.Consumables {
    [CreateAssetMenu(fileName = "SpeedPotion", menuName = "ScriptableObject/Consumables/SpeedPotion")] 
    public class SpeedPotion : Consumable  //hérite de consommable ou équipement dépendant du type 
    {

        //par défaut a un nom, prix, sprite, tu peux ajouter d'autre type en faisant des variables public comme healAmount ici 
        public override void Use()
        {
            if (UnitLibData.selectedUnit != null)
            {
                UnitLibData.selectedUnit.AddEffect(0, 3, 20f);

                base.Use();
            }

        }
    }
}