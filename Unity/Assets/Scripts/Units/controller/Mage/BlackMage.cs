using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class BlackMage : Wizard
{
    
    
    public BlackMage(AbstractUnit body) : base(body)
    {
        MultiplierScore = 2f;
        basisAttack *= 2;
    }
    
}
