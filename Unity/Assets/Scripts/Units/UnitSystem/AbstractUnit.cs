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
            for (int i = 0; i < Mathf.Sqrt(entityCount); i++) {
                for (int j = 0; j < Mathf.Sqrt(entityCount); j++) {
                    if (counterInstance <= entityCount) {
                        Entity entityGo = Instantiate(entityModel, transform);
                        entityGo.transform.localPosition = new Vector3(i+entityScale.x,0,j+entityScale.z);
                        entities[counterInstance++] = entityGo;
                    }
                }
            }
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
