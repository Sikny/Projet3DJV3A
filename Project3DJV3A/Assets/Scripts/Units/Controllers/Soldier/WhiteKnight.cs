using System.Collections;
using System.Collections.Generic;
using Units;
using Units.Controllers.Soldier;
using UnityEngine;

public class WhiteKnight : Soldier
{
    
    
    public WhiteKnight(AbstractUnit body) : base(body)
    {
        MultiplierScore = 4f;
        basisDefense *= 4;
    }
    
}
