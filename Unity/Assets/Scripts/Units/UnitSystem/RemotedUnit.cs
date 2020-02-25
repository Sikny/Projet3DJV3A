using System;
using TerrainGeneration;
using Units.proto;
using UnityEngine;

namespace Units.UnitSystem {
    public class RemotedUnit : AbstractUnit {
        private int _isWalkable = 1;
        private bool _isSelected;
    
        private Color _color = Color.cyan;
    
        public override bool Init(Entity entityModel, int entityCountP) {
            bool value = base.Init(entityModel, entityCountP);
            speedEntity = 1.0f;
            _isSelected = true;
        
            foreach (var entity in entities) {
                print(entity);
                entity.InitColor(_color);
            }

            gameObject.layer = 9;    // allied units

            return value;
        }

        public override void UpdateUnit() {
            /*if (isSelected) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                if (!SetTargetPosition()) return;
            }
        }*/
            if(isMoving)
                Move();
            UpdateGameObject();
        }

        public override bool Kill() {
            return true;
        }

        protected override void Attack(AbstractUnit anotherUnit) {
            
        }
    
        public bool SetTargetPosition() {
            Ray ray = UnitLibData.cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
        
            if (Physics.Raycast(ray, out hit, 100, 1 << 4) ||
                !Physics.Raycast(ray, out hit, 100, UnitLibData.groundMask)) return false;
            else {
                if (hit.transform.gameObject.layer == 4) //water 
                    _isWalkable = 0;
                else if (hit.transform.gameObject.layer == 8) //ground 
                    _isWalkable = 1;

                float xHit = Mathf.Floor(hit.transform.position.x);
                float zHit = Mathf.Floor(hit.transform.position.z);
                TerrainGrid.Instance.TileX = (int) xHit + (TerrainGrid.Width / 2);
                TerrainGrid.Instance.TileZ = (int) zHit + (TerrainGrid.Height / 2);

                Vector2 startPosition = new Vector2(Mathf.Floor(position.x) + (TerrainGrid.Width / 2),
                    Mathf.Floor(position.z) + (TerrainGrid.Height / 2));
                Vector2 endPosition = new Vector2(TerrainGrid.Instance.TileX, TerrainGrid.Instance.TileZ);

                //pathFinder.BuildPath(startPosition, endPosition, isWalkable);


                //targetPosition = hit.point + offsetPosition;
                targetPosition = new Vector3(Mathf.Floor(hit.transform.position.x)-0.5f, 1,
                    Mathf.Floor(hit.transform.position.z)-0.5f) ;


                lookAtTarget = new Vector3(targetPosition.x - position.x, position.y,
                    targetPosition.z - position.z);

                //Vector of unit to point 
                Vector3 unitToTarget = (targetPosition - position);
                unitToTarget.Normalize();
                //Dot product of the two vectors and Acos 
                //rotationAngle = (Mathf.Acos(Vector3.Dot(unitToTarget, gameObject.transform.forward)) *
                //      (180 / Mathf.PI));
                //isRight = Vector3.Dot(unitToTarget, gameObject.transform.right) > 0;

                isMoving = true;
                isTurning = true;
                return true;
            }
        }

        public void Select() {
            foreach (var entity in entities) {
                entity.meshRenderer.material.color = Color.yellow;
            }
        }

        public void Deselect() {
            foreach (var entity in entities) {
                entity.ResetColor();
            }
        }
    }
}
