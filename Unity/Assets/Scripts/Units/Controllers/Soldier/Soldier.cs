using Terrain;
using UnityEngine;
using Utility;

namespace Units.Controllers.Soldier {
    public class Soldier : Controller {
        private const float TickAttack = 0.10f; // 10 per second
        private bool _playingSound;

        public Soldier(AbstractUnit body) : base(body) {
            basisSpeed = 0.8f;
            basisAttack = 1.0f;
            basisDefense = 1.0f;
        }

        public override bool Interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            deltaTime += UnitLibData.deltaTime;

            bool result = false;
            if (body.isMoving)
                result = Move(isRemoted, target, positionTarget);

            if (deltaTime >= TickAttack) {
                if (target == null) return result;
                if (Vector3.Distance(body.GetPosition(), target.GetPosition()) <= 3) {
                    if (!_playingSound) {
                        GameSingleton.Instance.soundManager.Play("Slash");
                    }

                    body.Attack(target, GetAttackUnit(target));
                }

                deltaTime -= TickAttack;
            }

            return result;
        }

        private bool Move(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            if (!isRemoted && target == null) return false;

            int ind = 0;
            if (!isRemoted) {
                Entity entityTarget = GetFirstLivingEntity();
                if (entityTarget == null) return false;
                if (Vector3.Distance(entityTarget.transform.position, target.GetPosition()) <= 2.0f) {
                    Vector3 destination = TerrainGrid.Instance.GetClosestValidPosition(entityTarget.transform.position);
                    if (destination.x > -0.1f) {    // if < 0 not valid
                        LockPosition(destination);
                    }
                }
            }

            bool isOnDest = false;
            
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if (body.entities[ind] != null) {
                        Vector3 position = positionTarget + Vector3.right * x + Vector3.forward * y;
                        if (body.entities[ind].aStarEntity.MoveTo(position, GameSingleton.Instance.aStarHandler)) {
                            body.SetPosition(positionTarget);
                            isOnDest = true;
                        }
                    }
                    ind++;
                }
            }

            return isOnDest;
        }
    }
}