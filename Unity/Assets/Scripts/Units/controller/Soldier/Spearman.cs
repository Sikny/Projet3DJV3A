using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Spearman : Soldier
{
    
    
    public Spearman(AbstractUnit body) : base(body)
    {
        MultiplierScore = 2f;
        basisSpeed *= 2;
    }
    
}
