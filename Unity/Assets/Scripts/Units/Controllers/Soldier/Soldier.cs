using System.Collections;
using Terrain;
using UnityEngine;
using Utility;
using Cursor = Terrain.Cursor;

namespace Units.Controllers.Soldier {
    public class Soldier : Controller {
        private const float TickAttack = 0.10f; // 10 per second
        private bool _playingSound;

        public Soldier(AbstractUnit body) : base(body) {
            basisSpeed = 0.8f;
            basisAttack = 1.0f;
            basisDefense = 1.0f;
        }

        public override void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            deltaTime += UnitLibData.deltaTime;

            if (body.isMoving)
                Move(isRemoted, target, positionTarget);

            if (deltaTime >= TickAttack) {
                if (target == null) return;
                if (Vector3.Distance(body.GetPosition(), target.GetPosition()) <= 3) {
                    if (!_playingSound) {
                        GameSingleton.Instance.soundManager.Play("Slash");
                    }

                    body.Attack(target, getAttackUnit(target));
                }

                deltaTime -= TickAttack;
            }
        }

        private void Move(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            if (target == null) return;

            int ind = 0;
            
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    Vector3 position = positionTarget + Vector3.right * x + Vector3.forward * y;
                    if (body.entities[ind] == null) continue;
                    if (body.entities[ind++].aStarEntity.MoveTo(position, GameSingleton.Instance.aStarHandler)) {
                        body.SetPosition(positionTarget);
                    }
                }
            }

            /*for (int i = body.entityCount - 1; i >= 0; i--)
            { 
                GameSingleton.Instance.levelManager.loadedLevel.aStarHandler.UpdateTransform(body.entities[i].transform, positionTarget, basisSpeed);
            }*/
            //GameSingleton.Instance.levelManager.loadedLevel.aStarHandler.UpdateTransform(body, positionTarget, basisSpeed);
            //SystemUnit.UpdateTransform(body, positionTarget, speedEntity);

            //updatePathMove();

            //velocity = position - last;
        }
    }
}