using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using Units;
using UnityEngine;

namespace Units{
    public class Zombie : Controller
    {
        private const float TICK_ATTACK= 0.10f; // 10 per second
        
        public Zombie(AbstractUnit body) : base(body)
        {
            speedEntity = 0.7f;
        }
        
        public override void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget)
        {
            deltaTime += UnitLibData.deltaTime;

            if(body.isMoving)
                Move(isRemoted, target, positionTarget);
            
            if (deltaTime >= TICK_ATTACK)
            {
                if (Vector3.Distance(body.GetPosition(), target.GetPosition()) <= 3) {
                    body.Attack(target);
                }
                deltaTime -= TICK_ATTACK;
            }
        }
        
        private void Move(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {

            if (target == null) return;
            if (!isRemoted && Vector3.Distance(body.GetPosition(), target.GetPosition()) <= 3)
            {
                return; 
            }
            Vector3 last = body.GetPosition();
            Vector3 posTarget = isRemoted ? positionTarget : target.GetPosition();
            body.SetPosition(Vector3.MoveTowards(last, posTarget, UnitLibData.speed * Time.deltaTime * speedEntity));
            
            //velocity = position - last;
        }
    }
}