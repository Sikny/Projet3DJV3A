using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Units{
    public class Soldier : Controller
    {
        private const float TICK_ATTACK= 0.10f; // 10 per second
        private bool _playingSound;
        public Soldier(AbstractUnit body) : base(body)
        {
            basisSpeed = 0.8f;
            basisAttack = 1.0f;
            basisDefense = 1.0f;
            
            itineraireNumberRemain = 0;
        }
        
        public override void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget)
        {
            deltaTime += UnitLibData.deltaTime;

            if(body.isMoving)
                Move(isRemoted, target, positionTarget);
            
            if (deltaTime >= TICK_ATTACK) {
                if (target == null) return;
                if (Vector3.Distance(body.GetPosition(), target.GetPosition()) <= 3)
                {
                    if (!_playingSound)
                    {
                        GameSingleton.Instance.soundManager.Play("Slash");
                    }
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

            /*for (int i = body.entityCount - 1; i >= 0; i--)
            { 
                GameSingleton.Instance.levelManager.loadedLevel.aStarHandler.UpdateTransform(body.entities[i].transform, positionTarget, speedEntity);
            }*/
            GameSingleton.Instance.levelManager.loadedLevel.aStarHandler.UpdateTransform(body, positionTarget, speedEntity);
            //SystemUnit.UpdateTransform(body, positionTarget, speedEntity);

            //updatePathMove();

            //velocity = position - last;
        }

        IEnumerator WaitForSound()
        {
            _playingSound = true; 
            yield return new WaitForSeconds(1);
            _playingSound = false;
        }
    }
}