﻿using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class WhiteMage : Wizard
{
    
    
    public WhiteMage(AbstractUnit body) : base(body)
    {
        MultiplierScore = 2f;
        basisDefense *= 2;
    }
    
}
