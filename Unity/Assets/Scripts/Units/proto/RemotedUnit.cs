using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

public class RemotedUnit : AbstractUnit
{
    public RemotedUnit(int numberEntity) : 
        base(numberEntity,new Vector3(-2,1,0))
    {
            
    }
        
    public bool init(GameObject gameobjectModel)
    {
        return base.init(gameobjectModel);
    }

    public override bool kill()
    {
        return true;
    }
}
