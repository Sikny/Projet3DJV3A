using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Equipment", menuName = "ScriptableObjects/Equipment")]
public class Equipment : ScriptableObject
{
    new public string name = "New Equipment";
    public Sprite icon = null;
    public int price = 10;
    //public String description = ""; 
    //public int statDif
    
}