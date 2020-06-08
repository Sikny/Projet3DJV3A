using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Catapultist : Archer
{
    
    
    public Catapultist(AbstractUnit body) : base(body)
    {
        basisAttack *= 8;
        TICK_ATTACK /= 2;
    }
    
}
