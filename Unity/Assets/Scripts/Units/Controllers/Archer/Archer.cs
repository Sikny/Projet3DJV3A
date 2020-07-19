using Terrain;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Units.Controllers.Archer {
    public class Archer : Controller {
        protected float tickAttack;

        protected float accuracy; // Max probability to touch target
        private const float ZoneAccuracyDispersement = 0.2f;
        private const float OptimalDistance = 6f;

        private bool _canShoot = true;

        public Archer(AbstractUnit body) : base(body) {
            basisSpeed = 0.5f;
            basisAttack = 2f;
            basisDefense = 2f;
            accuracy = 0.5f;
            tickAttack = 2f;
        }

        public override bool Interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            if (target == null) return false;
            deltaTime += UnitLibData.deltaTime;

            bool result = false;
            if (body.isMoving)
                result = Move(isRemoted, target, positionTarget);

            if (_canShoot && deltaTime >= tickAttack) {
                // TODO CHANGE THIS
                for (int i = 0; i < body.GetNumberAlive(); i++) // Chaque entité de l'unité
                {
                    float distance = Vector3.Distance(positionTarget, body.GetPosition());
                    float ceilAccuracy =
                        accuracy * Mathf.Exp(Mathf.Pow(-ZoneAccuracyDispersement * (distance - OptimalDistance),
                            2)); // calcul de la précision
                    if (ceilAccuracy >= Random.Range(0f, 1.0f)) {
                        GameSingleton.Instance.soundManager.Play("ArcherAttack");
                        body.Attack(target, GetAttackUnit(target));
                    }

                    //get efficiency type 
                }

                deltaTime -= tickAttack;
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
                        if (body.entities[ind].aStarEntity.MoveTo(position, GameSingleton.Instance.aStarHandler, GetVitessUnit())) {
                            body.SetPosition(positionTarget);
                            isOnDest = true;
                        }
                    }
                    ind++;
                }
            }
            /*} else {
                Vector3 last = body.GetPosition();
                Vector3 posTarget = target.GetPosition();
                if (Vector3.Distance(last, posTarget) <= OptimalDistance - 0.2f) {
                    body.transform.position = Vector3.MoveTowards(last, posTarget, -GetVitessUnit());
                    _canShoot = false;
                }
                else if (Vector3.Distance(last, posTarget) >= OptimalDistance + 0.2f) {
                    body.transform.position = Vector3.MoveTowards(last, posTarget, GetVitessUnit());
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