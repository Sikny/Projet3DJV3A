using System.Collections;
using System.Collections.Generic;
using Units.utils;
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

            Effect effect = body.GetEffect(0);

            int bonusLevel = effect.IdEffect == -1 ? 0 : effect.LevelEffect;


            return baseVitess + bonusLevel * baseVitess * 0.5f;
        }
        protected float getAttackUnit(AbstractUnit target)
        {
            float baseAttack = basisAttack;

            Effect effect = target.GetEffect(1); //defense

            int bonusLevel = effect.IdEffect == -1 ? 1 : effect.LevelEffect + 1;


            return basisAttack/bonusLevel;
        }
    }
}
