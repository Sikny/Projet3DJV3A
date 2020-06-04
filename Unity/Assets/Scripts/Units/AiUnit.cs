using Game;
using Units;
using UnityEngine;
using Utility;

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
                if (_unitTarget != null) 
                    Debug.Log("posTarget="+_unitTarget.GetPosition());
                    brain.calculatePath(_unitTarget.GetPosition());
            }
            else {
                targetPosition = _unitTarget.GetPosition();
                isMoving = true;
            }
            
            brain.interract(false, _unitTarget, targetPosition);

            UpdateTimeoutEffects();
            
            UpdateGameObject();
        }

        public override void Attack(AbstractUnit anotherUnit, float damage) {
            int indexEntityAttack = Random.Range(0, entityCount);
            Entity entityAttack = this.GetEntity(indexEntityAttack);
            float coef = GetEfficientCoef(this, anotherUnit);
            int efficientCoef = GetEfficiencyType(coef);

            if (anotherUnit.GetNumberAlive() > 1) {
                int indexEntityDefense = Random.Range(1, entityCount);
                Entity entityDefense = anotherUnit.GetEntity(indexEntityDefense);

                if (entityAttack == null || entityDefense == null) return;

                
                int life = entityDefense.ChangeLife((int)(-1 * entityAttack.GetStrength() * damage * coef), efficientCoef);
                if (life == 0) {
                    anotherUnit.PopEntity(indexEntityDefense);
                }
            }
            else if(anotherUnit.GetNumberAlive() == 1) {
                if (entityAttack != null) {
                    anotherUnit.GetEntity(0).ChangeLife(-100, efficientCoef);
                    anotherUnit.PopEntity(0); // Le leader est attrapé
                    _unitTarget = null; //important pour indiquer à l'IA de commencer de nouvelles recherches
                }
            }
        }
        private int GetEfficiencyType(float efficientCoef)
        {
            int res = 0; 
            //display particule on anotherUnit (targeted unit) 
            if (efficientCoef == 1f)
            {
                return res;
                //attack is neutral gray
            }
            else if (efficientCoef < 1f)
            {
                res = -1;
                //attack is unefficient red 
            }
            else
            {
                res = 1;
                //attack is efficient green 
            }

            return res; 
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

        public override void Kill() {
            GameSingleton.Instance.GetPlayer().gold += 150;
            base.Kill();
        }
    }
}
