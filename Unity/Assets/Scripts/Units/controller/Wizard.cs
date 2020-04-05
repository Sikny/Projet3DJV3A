using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Wizard : Controller
{
    
    private const float TICK_ATTACK= 10f; // 10 per second

    private const float ACCURACY = 0.5f; // Max probability to touch target
    private const float ZONE_ACCURACY_DISPERSEMENT = 0.2f;
    private const float OPTIMAL_DISTANCE = 4.5f;

    private bool canShoot = false;

    public Wizard(AbstractUnit body) : base(body)
    {
        speedEntity = 0.5f;
        basisAttack = 10f;
    }

    public override void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget)
    {
        deltaTime += UnitLibData.deltaTime;

        if(body.isMoving)
            Move(isRemoted, target, positionTarget);
            
        if (canShoot && deltaTime >= TICK_ATTACK)
        {
            body.Attack(target, basisAttack);

            deltaTime -= TICK_ATTACK;
        }
    }
    private void Move(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {

        if (target == null) return;

        if (isRemoted)
        {
            Vector3 last = body.GetPosition();
            Vector3 posTarget = positionTarget;
            body.SetPosition(Vector3.MoveTowards(last, posTarget, UnitLibData.speed * Time.deltaTime * speedEntity));
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
                body.SetPosition(Vector3.MoveTowards(last, posTarget, -UnitLibData.speed * Time.deltaTime * speedEntity));
                canShoot = false;
            }else if (Vector3.Distance(last, posTarget) >= OPTIMAL_DISTANCE + 1f)
            {
                body.SetPosition(Vector3.MoveTowards(last, posTarget, UnitLibData.speed * Time.deltaTime * speedEntity));
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
