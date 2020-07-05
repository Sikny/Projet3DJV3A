using System.Collections;
using System.Collections.Generic;
using Units;
using Units.Controllers.Archer;
using UnityEngine;

public class Sniper : Archer
{
    
    
    public Sniper(AbstractUnit body) : base(body)
    {
        MultiplierScore = 4f;
        tickAttack /= 2;
        basisAttack *= 4;
        accuracy = int.MaxValue; 
    }
    // TODO le faire tirer de loin
    
}
