using Units.proto;
using UnityEngine;

namespace Units.UnitSystem  {
    public class AiUnit : AbstractUnit {
        private float _deltaTime;
        private RemotedUnit _unitTarget;

        private const float TickAttack = 0.10f; //PARAM OF DIFFICULTY

        public override bool Init(Entity entityModel, int entityCountP) {
            _deltaTime = 0.0f;
            speedEntity = 0.7f;
            _unitTarget = null;
            return base.Init(entityModel, entityCountP);
        }

        public override void UpdateUnit() {
            _deltaTime += UnitLibData.deltaTime;
            
            if (_unitTarget == null) {
                _unitTarget = GuessTheBestUnitToTarget();
            }
            else {
                targetPosition = _unitTarget.GetPosition();
                isMoving = true;
            }
            if(isMoving)
                Move();
            if (_deltaTime >= TickAttack) {
                if (Vector3.Distance(position, targetPosition) <= 3) {
                    Attack(_unitTarget);
                }
                _deltaTime -= TickAttack;
            }
            UpdateGameObject();
        }

        protected override void Attack(AbstractUnit anotherUnit) {
            int indexEntityAttack = Random.Range(0, entityCount);
            Entity entityAttack = this.GetEntity(indexEntityAttack);
            Debug.Log(anotherUnit.GetNumberAlive());
            if (anotherUnit.GetNumberAlive() > 1) {
                int indexEntityDefense = Random.Range(1, entityCount);
                Entity entityDefense = anotherUnit.GetEntity(indexEntityDefense);

                if (entityAttack == null || entityDefense == null) return;

                int life = entityDefense.ChangeLife(-1 * entityAttack.GetStrength());
                if (life == 0) {
                    anotherUnit.PopEntity(indexEntityDefense);
                }
            }
            else if(anotherUnit.GetNumberAlive() == 1) {
                if (entityAttack != null) {
                    anotherUnit.GetEntity(0).ChangeLife(-100);
                    anotherUnit.PopEntity(0); // Le leader est attrapé
                }
            }
        }
        
        private RemotedUnit GuessTheBestUnitToTarget() {
            RemotedUnit best = null;
            float bestDistance = float.PositiveInfinity;
            foreach (var unit in UnitLibData.units) {
                if (unit is RemotedUnit) {
                    float distance = Vector3.Distance(this.position, unit.GetPosition());
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
        public override bool Kill() {
            return true;
        }
    }
}
