using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public abstract class Controller
    {

        protected AbstractUnit body;
        protected float speedEntity;
        protected float basisAttack;
        
        protected float deltaTime;
        protected Controller(AbstractUnit body)
        {
            this.body = body;
            deltaTime = 0.0f;
        }

        public abstract void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget);

        protected float getVitessUnit()
        {
            float baseVitess = UnitLibData.speed * Time.deltaTime * speedEntity;

            Effect effect = body.getEffect(0);

            int bonusLevel = effect.IdEffect == -1 ? 0 : effect.LevelEffect;


            return baseVitess + bonusLevel * baseVitess * 0.5f;
        }
    }
}
