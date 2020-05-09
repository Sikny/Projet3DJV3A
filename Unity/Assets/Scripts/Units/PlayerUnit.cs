using TerrainGeneration;
using UnityEngine;
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

            if (_unitTarget == null) _unitTarget = GuessTheBestUnitToTarget();

            brain.interract(true,_unitTarget, targetPosition);
            
            UpdateTimeoutEffects();
            
            UpdateGameObject();
            
        }

        public override void Attack(AbstractUnit anotherUnit, float damage) {
            int indexEntityAttack = Random.Range(0, entityCount);
            Entity entityAttack = GetEntity(indexEntityAttack);
            if (anotherUnit.GetNumberAlive() > 1) {
                int indexEntityDefense = Random.Range(1, entityCount);
                Entity entityDefense = anotherUnit.GetEntity(indexEntityDefense);

                if (entityAttack == null || entityDefense == null) return;

                int life = entityDefense.ChangeLife((int)(-1 * entityAttack.GetStrength()*damage* GetEfficientCoef(this, anotherUnit)));
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
    
        public void SetTargetPosition(float yPos) {
            Ray ray = UnitLibData.cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 100, 1 << 4) ||
                !Physics.Raycast(ray, out hit, 100, UnitLibData.groundMask))
                return;

            float xHit = Mathf.Floor(hit.point.x);
            float zHit = Mathf.Floor(hit.point.z);
            TerrainGrid.Instance.TileX = (int) xHit + (TerrainGrid.Width / 2);
            TerrainGrid.Instance.TileZ = (int) zHit + (TerrainGrid.Height / 2);

            targetPosition = new Vector3(Mathf.Floor(hit.point.x)-0.5f, yPos,
                Mathf.Floor(hit.point.z)-0.5f) ;
            
            //Vector of unit to point 
            Vector3 unitToTarget = (targetPosition - position);
            unitToTarget.Normalize();

            isMoving = true;
            isTurning = true;
        }
    }
}
