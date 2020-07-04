using System.Collections;
using System.Collections.Generic;
using Units;
using Units.controller.Archer;
using UnityEngine;

public class Catapultist : Archer
{
    
    
    public Catapultist(AbstractUnit body) : base(body)
    {
        MultiplierScore = 4f;
        basisAttack *= 8;
        TICK_ATTACK /= 2;
    }
    
}
