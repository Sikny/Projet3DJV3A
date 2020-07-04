using System.Collections;
using System.Collections.Generic;
using Units;
using Units.controller.Archer;
using UnityEngine;

public class Hunter : Archer
{
    
    
    public Hunter(AbstractUnit body) : base(body)
    {
        MultiplierScore = 2f;
        accuracy = 1f;
        basisAttack *= 2;
    }
    
}
