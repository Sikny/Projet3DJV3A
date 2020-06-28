using UnityEngine;
using Utility;

namespace Units.Controllers.Mage {
    public class Wizard : Controller
    {
    
        private const float TICK_ATTACK= 4f; // 10 per second

        private const float ACCURACY = 0.5f; // Max probability to touch target
        private const float ZONE_ACCURACY_DISPERSEMENT = 0.2f;
        private const float OPTIMAL_DISTANCE = 4.5f;

        private bool canShoot = false;

    
        public Wizard(AbstractUnit body) : base(body)
        {
            basisSpeed = 0.5f;
            basisAttack = 4f;
            basisDefense = 1;

            //upgrades.Add(EntityType.something);
        }

        public override void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget)
        {
            deltaTime += UnitLibData.deltaTime;

            if(body.isMoving)
                Move(isRemoted, target, positionTarget);
            
            if (canShoot && deltaTime >= TICK_ATTACK)
            {
                GameSingleton.Instance.soundManager.Play("MageAttack");

                for (int i = 0; i < 9; i++)
                {
                    body.Attack(target, getAttackUnit(target));
                }

                deltaTime -= TICK_ATTACK;
            }
        }
        private void Move(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {

            if (target == null) return;

            if (isRemoted)
            {
                Vector3 last = body.GetPosition();
                Vector3 posTarget = positionTarget;
                body.SetPosition(Vector3.MoveTowards(last, posTarget, getVitessUnit()));
                float dist = Vector3.Distance(last, posTarget);
                if (OPTIMAL_DISTANCE - 1f <= dist && dist <= 1f + OPTIMAL_DISTANCE)
                    canShoot = true;
                else
                    canShoot = false;
            }
            else
            {
                Vector3 last = body.GetPosition();
                Vector3 posTarget = target.GetPosition();
                if(Vector3.Distance(last, posTarget) <= OPTIMAL_DISTANCE-1f)
                {
                    body.SetPosition(Vector3.MoveTowards(last, posTarget, -getVitessUnit()));
                    canShoot = false;
                }else if (Vector3.Distance(last, posTarget) >= OPTIMAL_DISTANCE + 1f)
                {
                    body.SetPosition(Vector3.MoveTowards(last, posTarget, getVitessUnit()));
                    canShoot = false;
                }
                else
                {
                    // Ok
                    canShoot = true;
                }
            }  
        }
    }
}
