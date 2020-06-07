using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class RedMage : Wizard
{
    
    
    public RedMage(AbstractUnit body) : base(body)
    {
        basisAttack *= 2;
        basisDefense *= 2;
    }
    
}
