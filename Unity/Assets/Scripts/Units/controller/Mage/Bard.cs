using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Bard : Wizard
{
    
    
    public Bard(AbstractUnit body) : base(body)
    {
        basisDefense *= 4;
    }
    
}
