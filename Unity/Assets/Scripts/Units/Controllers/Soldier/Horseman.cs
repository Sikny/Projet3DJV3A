using System.Collections;
using System.Collections.Generic;
using Units;
using Units.Controllers.Soldier;
using UnityEngine;

public class Horseman : Soldier
{
    
    
    public Horseman(AbstractUnit body) : base(body)
    {
        MultiplierScore = 4f;
        basisSpeed *= 4;
    }
    
}
