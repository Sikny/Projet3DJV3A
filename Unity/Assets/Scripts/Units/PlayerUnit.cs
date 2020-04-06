using TerrainGeneration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units {
    public class PlayerUnit : AbstractUnit {
        private int _isWalkable = 1;
        private bool _isSelected;

        private Color _color = Color.cyan;
        
        private float _deltaTime;

        private const float TickAttack = 0.10f; //PARAM OF DIFFICULTY
    
        public override bool Init(EntityType idType, Entity entityModel, int entityCountP) {
            bool value = base.Init(idType, entityModel, entityCountP);
            speedEntity = 1.0f;
            _isSelected = true;
            _deltaTime = 0.0f;
        
            foreach (var entity in entities) {
                entity.InitColor(_color);
            }
            gameObject.layer = 9;    // allied units
            return value;
        }

        public override void UpdateUnit() {
            if (!initialized) return;
            _deltaTime += UnitLibData.deltaTime;

            if (_unitTarget == null) _unitTarget = GuessTheBestUnitToTarget();

            brain.interract(true,_unitTarget, targetPosition);
            
            updateTimeoutEffects();
            
            UpdateGameObject();
            
        }

        public override bool Kill()
        {
            return true;
        }

        public override void Attack(AbstractUnit anotherUnit, float damage) {
            int indexEntityAttack = Random.Range(0, entityCount);
            Entity entityAttack = this.GetEntity(indexEntityAttack);
            if (anotherUnit.GetNumberAlive() > 1) {
                int indexEntityDefense = Random.Range(1, entityCount);
                Entity entityDefense = anotherUnit.GetEntity(indexEntityDefense);

                if (entityAttack == null || entityDefense == null) return;

                int life = entityDefense.ChangeLife((int)(-1 * entityAttack.GetStrength()*damage* getEfficientCoef(this, anotherUnit)));
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
    
        public void SetTargetPosition() {
            Ray ray = UnitLibData.cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 100, 1 << 4) ||
                !Physics.Raycast(ray, out hit, 100, UnitLibData.groundMask))
                return;
            if (hit.transform.gameObject.layer == 4) //water 
                _isWalkable = 0;
            else if (hit.transform.gameObject.layer == 8) //ground 
                _isWalkable = 1;

            float xHit = Mathf.Floor(hit.transform.position.x);
            float zHit = Mathf.Floor(hit.transform.position.z);
            TerrainGrid.Instance.TileX = (int) xHit + (TerrainGrid.Width / 2);
            TerrainGrid.Instance.TileZ = (int) zHit + (TerrainGrid.Height / 2);

            targetPosition = new Vector3(Mathf.Floor(hit.transform.position.x)-0.5f, 1,
                Mathf.Floor(hit.transform.position.z)-0.5f) ;


            //Vector of unit to point 
            Vector3 unitToTarget = (targetPosition - position);
            unitToTarget.Normalize();

            isMoving = true;
            isTurning = true;
        }

        public void Select() {
            foreach (var entity in entities) {
                if (entity == null) continue;
                entity.meshRenderer.material.color = Color.yellow;
            }
        }

        public void Deselect() {
            foreach (var entity in entities)
            {
                if (entity == null) continue;
                entity.ResetColor();
            }
        }
    }
}
