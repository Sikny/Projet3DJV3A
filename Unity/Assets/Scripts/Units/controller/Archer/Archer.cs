using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class Archer : Controller
{
    private const float TICK_ATTACK= 1.0f; // 10 per second

    private const float ACCURACY = 0.5f; // Max probability to touch target
    private const float ZONE_ACCURACY_DISPERSEMENT = 0.2f;
    private const float OPTIMAL_DISTANCE = 6f;

    private bool canShoot = true;
    
    public Archer(AbstractUnit body) : base(body)
    {
        speedEntity = 0.5f;
        basisAttack = 2.5f;
        
    }

    public override void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
        if (target == null) return;
        deltaTime += UnitLibData.deltaTime;

        if(body.isMoving)
            Move(isRemoted, target, positionTarget);
            
        if (canShoot && deltaTime >= TICK_ATTACK)
        {
            for (int i = 0; i < body.GetNumberAlive(); i++) // Chaque entité de l'unité
            {
                float distance = Vector3.Distance(positionTarget, body.GetPosition());
                float ceilAccuracy = ACCURACY * Mathf.Exp(Mathf.Pow(-ZONE_ACCURACY_DISPERSEMENT*(distance-OPTIMAL_DISTANCE),2)); // calcul de la précision
                if (ceilAccuracy >= Random.Range(0f, 1.0f))
                {
                    GameSingleton.Instance.soundManager.Play("ArcherAttack");
                    body.Attack(target, getAttackUnit(target));
                    
                }
                
                //get efficiency type 
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
            canShoot = true;
        }
        else
        {
            Vector3 last = body.GetPosition();
            Vector3 posTarget = target.GetPosition();
            if(Vector3.Distance(last, posTarget) <= OPTIMAL_DISTANCE-0.2f)
            {
                body.SetPosition(Vector3.MoveTowards(last, posTarget, -getVitessUnit()));
                canShoot = false;
            }else if (Vector3.Distance(last, posTarget) >= OPTIMAL_DISTANCE + 0.2f)
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
