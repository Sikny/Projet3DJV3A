using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Sniper : Archer
{
    
    
    public Sniper(AbstractUnit body) : base(body)
    {
        MultiplierScore = 4f;
        TICK_ATTACK /= 2;
        basisAttack *= 4;
        accuracy = int.MaxValue; 
    }
    // TODO le faire tirer de loin
    
}
