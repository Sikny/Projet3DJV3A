using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Equipment", menuName = "ScriptableObjects/Equipment")]
public class Equipment : Item
{
    public int stat;
    //public String description = ""; 
    //public int statDif

    public override void Use()
    {
        base.Use();
        //do stuff
    }
}