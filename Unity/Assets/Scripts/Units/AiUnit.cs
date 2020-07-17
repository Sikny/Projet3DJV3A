using Game;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Units  {
    public class AiUnit : AbstractUnit {
        private AbstractUnit _previousTarget;
        
        private float _timeLeft;

        public override bool Init(EntityType idType, Entity entityModel, int entityCountP)
        {
            bool initState = base.Init(idType, entityModel, entityCountP);
            unitTarget = null;
            gameObject.layer = 10;    // enemy units
            colliderObjectTransform.gameObject.layer = 10;
            return initState;
        }
        

        public override void UpdateUnit() {
            if (!initialized) return;

            _timeLeft -= Time.deltaTime;
            if (unitTarget == null || _timeLeft <= 0) {
                _previousTarget = unitTarget;
                
                unitTarget = GuessTheBestUnitToTarget();
                if (unitTarget != _previousTarget) {
                    brain.UnlockPosition();
                }
            }
            else {
                if (!brain.positionLocked){
                    targetPosition = unitTarget.GetPosition();
                    isMoving = true;
                }
            }
            
            brain.Interract(false, unitTarget, targetPosition);

            UpdateTimeoutEffects();
            
            UpdateGameObject();
        }

        public override void Attack(AbstractUnit anotherUnit, float damage) {
            int indexEntityAttack = Random.Range(1, entityCount);
            Entity entityAttack = GetEntity(indexEntityAttack);
            
            float coef = GetEfficientCoef(this, anotherUnit);
            int efficientCoef = GetEfficiencyType(coef);

            if (anotherUnit.GetNumberAlive() > 0) {
                int indexEntityDefense = Random.Range(0, entityCount);
                Entity entityDefense = anotherUnit.GetEntity(indexEntityDefense);

                if (entityAttack == null || entityDefense == null) return;

                int life = entityAttack.Attack(entityDefense, (int)(-1 * entityAttack.GetStrength()*damage* coef), efficientCoef);
                if (life == 0) {
                    anotherUnit.PopEntity(indexEntityDefense);
                }
            }
            else if(anotherUnit.GetNumberAlive() == 0) {
                if (entityAttack != null) {
                    unitTarget = null; //important pour indiquer à l'IA de commencer de nouvelles recherches
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
            _timeLeft = 2f;

            return best;
        }

        public override void Kill() {
            Player player = GameSingleton.Instance.GetPlayer();
            Player.Gamemode playerGamemode = player.gamemode;

            if (playerGamemode == Player.Gamemode.LEVEL)
                player.gold += 150;
            else
                player.arcadeGold += 150;
            base.Kill();
        }
    }
}
