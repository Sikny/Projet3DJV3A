using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Sniper : Archer
{
    
    
    public Sniper(AbstractUnit body) : base(body)
    {
        speedEntity = 0.5f;
        basisAttack = 10f;
    }
    
}
