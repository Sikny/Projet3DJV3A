using System.Collections;
using System.Collections.Generic;
using Units;
using Units.controller.Archer;
using UnityEngine;

public class MachineArc : Archer
{
    
    
    public MachineArc(AbstractUnit body) : base(body)
    {
        MultiplierScore = 4f;
        TICK_ATTACK *= 4;
        basisDefense *= 2;
    }
    
}
