using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

/*
 * sers a donner un chemin ou tu peux crée une nouvelle potion
 * filename = nom par défaut du fichier de l'objet
 * menuName = nom dans le menu quand tu crée
 *
 * pour crée un nouveau à partir de sa clique droit dans l'asset --> scriptable objec --> le nom que t'as mis dans menuName
 */
[CreateAssetMenu(fileName = "New Potion", menuName = "ScriptableObjects/Consummables/Potion")] 
public class Potion : Consummable  //hérite de consommable ou équipement dépendant du type 
{

    //par défaut a un nom, prix, sprite, tu peux ajouter d'autre type en faisant des variables public comme healAmount ici 
    public override void Use()
    {
        
        base.Use();
        
        if (UnitLibData._selectedUnit != null)
        {
            for (int i = 0; i < UnitLibData._selectedUnit.entityCount; i++)
            {
                if (UnitLibData._selectedUnit.GetEntity(i).getLife() > 0) // REGEN IF LIFE > 0
                    UnitLibData._selectedUnit.GetEntity(i).ResetLife();
            }
        }

    }
}
