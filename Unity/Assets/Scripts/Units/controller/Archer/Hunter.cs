using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Hunter : Archer
{
    
    
    public Hunter(AbstractUnit body) : base(body)
    {
        accuracy = 1f;
        basisAttack *= 2;
    }
    
}
