using CustomEvents;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Units {
    public class PlayerUnit : AbstractUnit {
        [SerializeField] private CustomEvent onPlayerUnitOnDestination;
        
        private float _timeLeft;
        
        public override bool Init(EntityType idType, Entity entityModel, int entityCountP) {
            bool value = base.Init(idType, entityModel, entityCountP);

            gameObject.layer = 9; // allied units
            colliderObjectTransform.gameObject.layer = 9;
            return value;
        }

        public override void UpdateUnit() {
            if (!initialized) return;
            _timeLeft -= Time.deltaTime;
            if (unitTarget == null || _timeLeft <= 0)
            {
                unitTarget = GuessTheBestUnitToTarget();
            }

            if (brain.Interract(true, unitTarget, targetPosition)) {
                onPlayerUnitOnDestination.Raise();
            }

            UpdateTimeoutEffects();

            UpdateGameObject();
        }

        public override void Attack(AbstractUnit anotherUnit, float damage) {
            int indexEntityAttack = Random.Range(1, entityCount);
            Entity entityAttack = GetEntity(indexEntityAttack);

            float coef = GetEfficientCoef(this, anotherUnit);
            int efficientCoef = GetEfficiencyType(coef);

            if (anotherUnit.GetNumberAlive() > 1) {
                int indexEntityDefense = Random.Range(1, entityCount);
                Entity entityDefense = anotherUnit.GetEntity(indexEntityDefense);

                if (entityAttack == null || entityDefense == null) return;

                int life = entityAttack.Attack(entityDefense, (int) (-1 * entityAttack.GetStrength() * damage * coef),
                    efficientCoef);
                if (life == 0) {
                    GameSingleton.Instance.GetPlayer().currentScore += (int)
                        (GetEfficientCoef(this, anotherUnit) * 200f *
                         anotherUnit.brain.MultiplierScore); // ajouter les types complexes
                    anotherUnit.PopEntity(indexEntityDefense);
                }
            }

            else if(anotherUnit.GetNumberAlive() == 1) {
                if (entityAttack != null) {
                    entityAttack.Attack(anotherUnit.GetEntity(0), -100, efficientCoef);
                    anotherUnit.PopEntity(0); // Le leader est attrapé
                    unitTarget = null; //important pour indiquer à l'IA de commencer de nouvelles recherches
                }
            }
        }

        private AiUnit GuessTheBestUnitToTarget()
        {
            AiUnit best = null;
            float bestDistance = float.PositiveInfinity;
            foreach (var unit in UnitLibData.units) {
                if (unit is AiUnit) {
                    float distance = Vector3.Distance(this.position, unit.GetPosition());
                    if (distance < bestDistance) {
                        bestDistance = distance;
                        best = (AiUnit) unit;
                    }
                }
            }
            _timeLeft = 0.7f;

            return best;
        }

        public void SetTargetPosition(Vector3 cursorPos) {
            targetPosition = new Vector3(cursorPos.x, SystemUnit.YPos, cursorPos.z);

            isMoving = true;
        }
    }
}