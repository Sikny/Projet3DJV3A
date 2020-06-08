using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Executionist : Soldier
{
    
    
    public Executionist(AbstractUnit body) : base(body)
    {
        basisAttack *= 4;
        basisSpeed /= 2;
    }
    
}
