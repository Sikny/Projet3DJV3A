using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Demonist : Wizard
{
    
    
    public Demonist(AbstractUnit body) : base(body)
    {
        basisAttack *= 4;
    }
    
}
