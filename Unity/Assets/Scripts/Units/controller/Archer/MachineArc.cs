using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class MachineArc : Archer
{
    
    
    public MachineArc(AbstractUnit body) : base(body)
    {
        TICK_ATTACK *= 4;
        basisDefense *= 2;
    }
    
}
