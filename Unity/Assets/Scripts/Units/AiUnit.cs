using Units;
using UnityEngine;

namespace Units  {
    /**
     * Cette classe répresente les unités contrôlées par une "IA" 
     */
    public class AiUnit : AbstractUnit {
        private float _deltaTime;
        private const float TickAttack = 0.10f; //PARAM OF DIFFICULTY

        public override bool Init(EntityType idType, Entity entityModel, int entityCountP) {
            bool initState = base.Init(idType, entityModel, entityCountP);
            _deltaTime = 0.0f;
            _unitTarget = null;
            gameObject.layer = 10;    // enemy units
            return initState;
        }
        

        public override void UpdateUnit() {
            if (!initialized) return;
            _deltaTime += UnitLibData.deltaTime;
            
            if (_unitTarget == null) {
                _unitTarget = GuessTheBestUnitToTarget();
            }
            else {
                targetPosition = _unitTarget.GetPosition();
                isMoving = true;
            }
            
            brain.interract(false, _unitTarget, targetPosition);

            updateTimeoutEffects();
            
            UpdateGameObject();
        }

        public override void Attack(AbstractUnit anotherUnit, float damage) {
            int indexEntityAttack = Random.Range(0, entityCount);
            Entity entityAttack = this.GetEntity(indexEntityAttack);
            if (anotherUnit.GetNumberAlive() > 1) {
                int indexEntityDefense = Random.Range(1, entityCount);
                Entity entityDefense = anotherUnit.GetEntity(indexEntityDefense);

                if (entityAttack == null || entityDefense == null) return;

                int life = entityDefense.ChangeLife((int)(-1 * entityAttack.GetStrength() * damage * getEfficientCoef(this, anotherUnit)));
                if (life == 0) {
                    anotherUnit.PopEntity(indexEntityDefense);
                }
            }
            else if(anotherUnit.GetNumberAlive() == 1) {
                if (entityAttack != null) {
                    anotherUnit.GetEntity(0).ChangeLife(-100);
                    anotherUnit.PopEntity(0); // Le leader est attrapé
                    _unitTarget = null; //important pour indiquer à l'IA de commencer de nouvelles recherches
                }
            }
        }
        
        private PlayerUnit GuessTheBestUnitToTarget() {
            PlayerUnit best = null;
            float bestDistance = float.PositiveInfinity;
            foreach (var unit in UnitLibData.units) {
                if (unit is PlayerUnit) {
                    float distance = Vector3.Distance(this.position, unit.GetPosition());
                    if (distance < bestDistance) {
                        bestDistance = distance;
                        best = (PlayerUnit)unit;
                    }
                }
            }
            return best;
        }
        
    }
}
