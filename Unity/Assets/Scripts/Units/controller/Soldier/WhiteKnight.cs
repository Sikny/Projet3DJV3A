using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class WhiteKnight : Soldier
{
    
    
    public WhiteKnight(AbstractUnit body) : base(body)
    {
        speedEntity = 0.8f;
        basisAttack = 1.0f;
    }
    
}
