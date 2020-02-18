using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  UnitSystem
{    
    public class AIUnit : AbstractUnit
    {
        public AIUnit(int numberEntity) : 
            base(numberEntity,new Vector3(2,1,0))
        {
            
        }
        
        public bool init()
        {
            
            return true;
        }

        public override bool kill()
        {
            return true;
        }
    }
}

