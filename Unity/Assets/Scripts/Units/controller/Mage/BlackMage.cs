using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class BlackMage : Wizard
{
    
    
    public BlackMage(AbstractUnit body) : base(body)
    {
        basisAttack *= 2;
    }
    
}
