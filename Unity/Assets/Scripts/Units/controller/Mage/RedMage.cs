using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class RedMage : Wizard
{
    
    
    public RedMage(AbstractUnit body) : base(body)
    {
        MultiplierScore = 4f;
        basisAttack *= 2;
        basisDefense *= 2;
    }
    
}
