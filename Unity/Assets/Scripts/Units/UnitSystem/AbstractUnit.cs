using System;
using Units.proto;
using UnityEngine;

namespace Units.UnitSystem {
    public abstract class AbstractUnit : MonoBehaviour {
        protected int entityCount;
        protected Entity[] entities;
        protected Vector3 position;

        protected Vector3 targetPosition;
        protected Vector3 lookAtTarget;
        
        protected bool isMoving;
        protected bool isTurning;
        /**
         * Utile pour savoir si le leader doit être attrapé
         */
        protected int livingEntityCount;

        // On peu imaginer que les ennemis vont moins vite
        protected float speedEntity;

        public virtual bool Init(Entity entityModel, int entityCountP) {
            entityCount = entityCountP;
            livingEntityCount = entityCountP;
            entities = new Entity[entityCountP];
            Vector3 entityScale = entityModel.transform.localScale;
            
            int counterInstance = 0;
            float sqrtEntityCount = (float) Math.Sqrt(entityCount);
            for (int i = 0; i < sqrtEntityCount; i++) {
                for (int j = 0; j < sqrtEntityCount; j++) {
                    if (counterInstance <= entityCount) {
                        Entity entityGo = Instantiate(entityModel, transform);
                        entityGo.transform.localPosition = new Vector3(i-entityScale.x*2,0,j-entityScale.z*2);
                        entities[counterInstance++] = entityGo;
                    }
                }
            }

            BoxCollider col = gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(sqrtEntityCount, 1, sqrtEntityCount);
            return true;
        }

        public void SetPosition(Vector3 pos) {
            position = pos;
            transform.position = pos;
        }

        protected abstract void Attack(AbstractUnit anotherUnit);

        public abstract void UpdateUnit();

        public abstract bool Kill();

        public Vector3 GetPosition() {
            return position;
        }
        
        protected virtual void Move() {
            if (targetPosition == null) return;
            position = Vector3.MoveTowards(position, targetPosition, UnitLibData.speed * Time.deltaTime * speedEntity);
            
            if (position == targetPosition) {
                isMoving = false;
            }
        }

        protected void UpdateGameObject() {
            if(livingEntityCount > 0)
                transform.position = position;
        }

        public Entity GetEntity(int index)
        {
            return entities[index];
        }

        public void PopEntity(int index) {
            livingEntityCount--;
            entities[index] = null;
        }

        public int GetNumberAlive() {
            return livingEntityCount;
        }
    }
    
}
