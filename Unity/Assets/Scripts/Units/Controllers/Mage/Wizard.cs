using Terrain;
using UnityEngine;
using Utility;

namespace Units.Controllers.Mage {
    public class Wizard : Controller {
        private const float TickAttack = 4f; // 10 per second
        private const float OptimalDistance = 4.5f;
        private bool _canShoot;

        public Wizard(AbstractUnit body) : base(body) {
            basisSpeed = 0.5f;
            basisAttack = 4f;
            basisDefense = 1;
            //upgrades.Add(EntityType.something);
        }

        public override bool Interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            deltaTime += UnitLibData.deltaTime;

            bool result = false;
            if (body.isMoving)
                result = Move(isRemoted, target, positionTarget);

            if (_canShoot && deltaTime >= TickAttack) {
                GameSingleton.Instance.soundManager.Play("MageAttack");

                for (int i = 0; i < 9; i++) {
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
                if (Vector3.Distance(entityTarget.transform.position, target.GetPosition()) <= OptimalDistance) {
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
            
            /*else {
                Vector3 last = body.GetPosition();
                Vector3 posTarget = target.GetPosition();
                if (Vector3.Distance(last, posTarget) <= OptimalDistance - 1f) {
                    body.SetPosition(Vector3.MoveTowards(last, posTarget, -GetVitessUnit()));
                    _canShoot = false;
                }
                else if (Vector3.Distance(last, posTarget) >= OptimalDistance + 1f) {
                    body.SetPosition(Vector3.MoveTowards(last, posTarget, GetVitessUnit()));
                    _canShoot = false;
                }
                else {
                    // Ok
                    _canShoot = true;
                }
            }*/
            return isOnDest;
        }
    }
}