using Units.proto;
using UnityEngine;

namespace UnitSystem {    
    public class AIUnit : AbstractUnit {
        private RemotedUnit unitTarget;
        public AIUnit(int numberEntity) : base(numberEntity,new Vector3(10,1,0)) {
            speedEntity = 0.7f;
            unitTarget = null;
        }
        public override bool init(GameObject gameobjectModel) {
            return base.init(gameobjectModel);
        }
        public override void update() {
            if (unitTarget == null) {
                unitTarget = GuessTheBestUnitToTarget();
            }
            else {
                targetPosition = unitTarget.getPosition();
                isMoving = true;
            }
            if(isMoving)
                Move();
            updateGameobject();
        }
        private RemotedUnit GuessTheBestUnitToTarget() {
            RemotedUnit best = null;
            float bestDistance = float.PositiveInfinity;
            foreach (var unit in UnitLibData.units) {
                if (unit is RemotedUnit) {
                    float distance = Vector3.Distance(this.position, unit.getPosition());
                    if (distance < bestDistance) {
                        bestDistance = distance;
                        best = (RemotedUnit)unit;
                    }
                }
            }
            return best;
        }
        protected override void Move() {
            if (Vector3.Distance(position, targetPosition) <= 3) {
                return;
            }
            base.Move();
        }
        public override bool kill() {
            return true;
        }
    }
}
