using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Arbalist : Archer
{
    
    
    public Arbalist(AbstractUnit body) : base(body)
    {
        MultiplierScore = 2f;
        TICK_ATTACK *= 2;
    }
    
}
