using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Spearman : Soldier
{
    
    
    public Spearman(AbstractUnit body) : base(body)
    {
        basisSpeed *= 2;
    }
    
}
