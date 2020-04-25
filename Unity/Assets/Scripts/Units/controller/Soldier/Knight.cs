using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Knight : Soldier
{
    
    
    public Knight(AbstractUnit body) : base(body)
    {
        speedEntity = 0.8f;
        basisAttack = 1.0f;
    }
    
}
