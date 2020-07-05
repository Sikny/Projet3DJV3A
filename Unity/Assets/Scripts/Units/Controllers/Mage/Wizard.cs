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
            if (target == null) return false;

            if (isRemoted) {
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
            }
            else {
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
            }
            return false;
        }
    }
}