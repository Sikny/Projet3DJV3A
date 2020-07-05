using System.Collections;
using System.Collections.Generic;
using Units;
using Units.Controllers.Archer;
using UnityEngine;

public class MachineArc : Archer
{
    
    
    public MachineArc(AbstractUnit body) : base(body)
    {
        MultiplierScore = 4f;
        tickAttack *= 4;
        basisDefense *= 2;
    }
    
}
