using Units.proto;
using UnityEngine;

namespace  UnitSystem  {
    public class AIUnit : AbstractUnit {
        private float deltaTime;
        private RemotedUnit unitTarget;

        private const float TICK_ATTACK = 0.10f; //PARAM OF DIFFICULTY
        
        public AIUnit(int numberEntity, Vector3 pos) : base(numberEntity, pos) {
            speedEntity = 0.7f;
            unitTarget = null;
        }
        
        public override bool init(GameObject gameobjectModel) {
            deltaTime = 0.0f;
            return base.init(gameobjectModel);
        }

        public override void update() {
            deltaTime += UnitLibData.deltaTime;
            
            if (unitTarget == null) {
                unitTarget = GuessTheBestUnitToTarget();
            }
            else {
                targetPosition = unitTarget.getPosition();
                isMoving = true;
            }
            if(isMoving)
                Move();
            if (deltaTime >= TICK_ATTACK) {
                if (Vector3.Distance(position, targetPosition) <= 3) {
                    attack(unitTarget);
                }
                deltaTime -= TICK_ATTACK;
            }
            updateGameobject();
        }

        protected override void attack(AbstractUnit anotherUnit) {
            int indexEntityAttack = Random.Range(0, numberEntity);
            Entity entityAttack = this.getEntity(indexEntityAttack);
            Debug.Log(anotherUnit.getNumberAlive());
            if (anotherUnit.getNumberAlive() > 1) {
                int indexEntityDefense = Random.Range(1, numberEntity);
                Entity entityDefense = anotherUnit.getEntity(indexEntityDefense);

                if (entityAttack == null || entityDefense == null) return;

                int life = entityDefense.ChangeLife(-1 * entityAttack.GetStrength());
                if (life == 0) {
                    anotherUnit.popEntity(indexEntityDefense);
                }
            }
            else if(anotherUnit.getNumberAlive() == 1) {
                if (entityAttack != null) {
                    anotherUnit.getEntity(0).ChangeLife(-100);
                    anotherUnit.popEntity(0); // Le leader est attrapé
                }
            }
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
