using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public abstract class Controller
    {

        protected AbstractUnit body;
        protected float speedEntity;
        
        protected float deltaTime;
        protected Controller(AbstractUnit body)
        {
            this.body = body;
            deltaTime = 0.0f;
        }

        public abstract void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget);
    }
}
