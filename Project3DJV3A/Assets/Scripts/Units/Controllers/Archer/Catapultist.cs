using System.Collections;
using System.Collections.Generic;
using Units;
using Units.Controllers.Archer;
using UnityEngine;

public class Catapultist : Archer
{
    
    
    public Catapultist(AbstractUnit body) : base(body)
    {
        MultiplierScore = 4f;
        basisAttack *= 8;
        tickAttack /= 8;
    }
    
}
