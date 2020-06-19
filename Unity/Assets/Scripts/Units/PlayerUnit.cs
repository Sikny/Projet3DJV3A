using System;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Units {
    public class PlayerUnit : AbstractUnit {
        public override bool Init(EntityType idType, Entity entityModel, int entityCountP) {
            bool value = base.Init(idType, entityModel, entityCountP);
            speedEntity = 1.0f;
        
            gameObject.layer = 9;    // allied units
            return value;
        }

        public override void UpdateUnit() {
            if (!initialized) return;

            if(_unitTarget != null)
            {
                brain.calculatePath(targetPosition);
            }
            else
            {
                _unitTarget = GuessTheBestUnitToTarget();
                if(_unitTarget != null)
                    brain.calculatePath(_unitTarget.GetPosition());
            }

            brain.interract(true,_unitTarget, targetPosition);
            
            UpdateTimeoutEffects();
            
            UpdateGameObject();
            
        }

        public override void Attack(AbstractUnit anotherUnit, float damage) {
            int indexEntityAttack = Random.Range(0, entityCount);
            Entity entityAttack = GetEntity(indexEntityAttack);
            
            float coef = GetEfficientCoef(this, anotherUnit);
            int efficientCoef = GetEfficiencyType(coef);

            if (anotherUnit.GetNumberAlive() > 1) {
                int indexEntityDefense = Random.Range(1, entityCount);
                Entity entityDefense = anotherUnit.GetEntity(indexEntityDefense);

                if (entityAttack == null || entityDefense == null) return;

                
                int life = entityAttack.Attack(entityDefense, (int)(-1 * entityAttack.GetStrength()*damage* coef), efficientCoef);
                if (life == 0)
                {
                    GameSingleton.Instance.GetPlayer().currentScore += (int)
                        (GetEfficientCoef(this, anotherUnit) * 200f * anotherUnit.brain.MultiplierScore); // ajouter les types complexes
                   // Debug.Log(GameSingleton.Instance.GetPlayer().currentScore);
                    anotherUnit.PopEntity(indexEntityDefense);
                }
            }
            else if(anotherUnit.GetNumberAlive() == 1) {
                if (entityAttack != null) {
                    entityAttack.Attack(anotherUnit.GetEntity(0), -100, efficientCoef);
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
        private AiUnit GuessTheBestUnitToTarget() {
            AiUnit best = null;
            float bestDistance = float.PositiveInfinity;
            foreach (var unit in UnitLibData.units) {
                if (unit is AiUnit) {
                    float distance = Vector3.Distance(this.position, unit.GetPosition());
                    if (distance < bestDistance) {
                        bestDistance = distance;
                        best = (AiUnit)unit;
                    }
                }
            }
            return best;
        }
        
    
        public void SetTargetPosition(Vector3 cursorPos) {

            targetPosition = new Vector3(cursorPos.x, SystemUnit.YPos, cursorPos.z) ;
            
            //Vector of unit to point 
            Vector3 unitToTarget = (targetPosition - position);
            unitToTarget.Normalize();

            isMoving = true;
            isTurning = true;
        }
    }
}
