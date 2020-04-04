using System.Collections;
using System.Collections.Generic;
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
    public int healAmount = 100;
    public override void Use()
    {
        base.Use();
        /*
         * met ton code pour appliquer les changement ici, comme c'est un scriptable object tu ne peux pas faire de référence direct dans unity
         * mais tu peux récupérer des instances tel que :
         *
         * private Exemple _exemple
         *
         *
         * private void Start()
         * {
         *    _exemple = Exemple.instance;   <-- récupère l'instance du singleton 
         *     
         * }
        */
    }
}
