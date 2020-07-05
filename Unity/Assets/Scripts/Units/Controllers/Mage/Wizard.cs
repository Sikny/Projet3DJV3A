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

        public override void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            deltaTime += UnitLibData.deltaTime;

            if (body.isMoving)
                Move(isRemoted, target, positionTarget);

            if (_canShoot && deltaTime >= TickAttack) {
                GameSingleton.Instance.soundManager.Play("MageAttack");

                for (int i = 0; i < 9; i++) {
                    body.Attack(target, getAttackUnit(target));
                }

                deltaTime -= TickAttack;
            }
        }

        private void Move(bool isRemoted, AbstractUnit target, Vector3 positionTarget) {
            if (target == null) return;

            if (isRemoted) {
                Vector3 last = body.GetPosition();
                Vector3 posTarget = positionTarget;
                body.SetPosition(Vector3.MoveTowards(last, posTarget, getVitessUnit()));
                float dist = Vector3.Distance(last, posTarget);
                if (OptimalDistance - 1f <= dist && dist <= 1f + OptimalDistance)
                    _canShoot = true;
                else
                    _canShoot = false;
            }
            else {
                Vector3 last = body.GetPosition();
                Vector3 posTarget = target.GetPosition();
                if (Vector3.Distance(last, posTarget) <= OptimalDistance - 1f) {
                    body.SetPosition(Vector3.MoveTowards(last, posTarget, -getVitessUnit()));
                    _canShoot = false;
                }
                else if (Vector3.Distance(last, posTarget) >= OptimalDistance + 1f) {
                    body.SetPosition(Vector3.MoveTowards(last, posTarget, getVitessUnit()));
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