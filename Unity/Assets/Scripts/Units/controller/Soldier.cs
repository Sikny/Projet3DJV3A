﻿using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using Units;
using UnityEngine;

namespace Units{
    public class Soldier : Controller
    {
        private const float TICK_ATTACK= 0.10f; // 10 per second
        
        public Soldier(AbstractUnit body) : base(body)
        {
            speedEntity = 0.8f;
            basisAttack = 1.0f;
        }
        
        public override void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget)
        {
            deltaTime += UnitLibData.deltaTime;

            if(body.isMoving)
                Move(isRemoted, target, positionTarget);
            
            if (deltaTime >= TICK_ATTACK)
            {
                if (Vector3.Distance(body.GetPosition(), target.GetPosition()) <= 3) {
                    body.Attack(target, getAttackUnit(target));
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
            body.SetPosition(Vector3.MoveTowards(last, posTarget, getVitessUnit()));
            
            //velocity = position - last;
        }
    }
}