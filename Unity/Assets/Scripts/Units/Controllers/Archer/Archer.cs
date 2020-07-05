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

        public override void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            if (target == null) return;
            deltaTime += UnitLibData.deltaTime;

            if (body.isMoving)
                Move(isRemoted, target, positionTarget);

            if (_canShoot && deltaTime >= tickAttack) {
                for (int i = 0; i < body.GetNumberAlive(); i++) // Chaque entité de l'unité
                {
                    float distance = Vector3.Distance(positionTarget, body.GetPosition());
                    float ceilAccuracy =
                        accuracy * Mathf.Exp(Mathf.Pow(-ZoneAccuracyDispersement * (distance - OptimalDistance),
                            2)); // calcul de la précision
                    if (ceilAccuracy >= Random.Range(0f, 1.0f)) {
                        GameSingleton.Instance.soundManager.Play("ArcherAttack");
                        body.Attack(target, getAttackUnit(target));
                    }

                    //get efficiency type 
                }

                deltaTime -= tickAttack;
            }
        }

        private void Move(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            if (target == null) return;

            if (isRemoted) {
                Vector3 last = body.GetPosition();
                Vector3 posTarget = positionTarget;
                
                body.transform.position = Vector3.MoveTowards(last, posTarget, getVitessUnit());
                _canShoot = true;
            }
            else {
                Vector3 last = body.GetPosition();
                Vector3 posTarget = target.GetPosition();
                if (Vector3.Distance(last, posTarget) <= OptimalDistance - 0.2f) {
                    body.transform.position = Vector3.MoveTowards(last, posTarget, -getVitessUnit());
                    _canShoot = false;
                }
                else if (Vector3.Distance(last, posTarget) >= OptimalDistance + 0.2f) {
                    body.transform.position = Vector3.MoveTowards(last, posTarget, getVitessUnit());
                    _canShoot = false;
                }
                else {
                    // Ok
                    _canShoot = true;
                }
            }
        }
    }
}