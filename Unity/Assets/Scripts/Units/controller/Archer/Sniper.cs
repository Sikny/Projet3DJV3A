using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Sniper : Archer
{
    
    
    public Sniper(AbstractUnit body) : base(body)
    {
        TICK_ATTACK /= 2;
        basisAttack *= 4;
        accuracy = int.MaxValue; 
    }
    
}
