using System;
using UnityEngine;

namespace Units {
    public abstract class AbstractUnit : MonoBehaviour {
        protected int entityCount;
        protected Entity[] entities;
        protected Vector3 position;
        protected Vector3 targetPosition;

        protected AbstractUnit _unitTarget;

        // The interaction controller (zombie, bowman, giant...)
        protected Controller brain;
        
        protected Vector3 velocity;
        
        public bool isMoving;
        protected bool isTurning;
        /**
         * Utile pour savoir si le leader doit être attrapé
         */
        protected int livingEntityCount;

        // On peu imaginer que les ennemis vont moins vite
        protected float speedEntity;

        protected Rigidbody rigidBody;

        public virtual bool Init(int idType,Entity entityModel, int entityCountP)
        {

            this.brain = getControllerFromId(idType);
            
            entityCount = entityCountP;
            livingEntityCount = entityCountP;
            entities = new Entity[entityCountP];
            
            velocity = new Vector3(0.0f,0.0f,0.0f);
            
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
            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionY|RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ ;
            return true;
        }

        // Init of unit's controller
        public Controller getControllerFromId(int id)
        {
            switch (id) 
            {
                // Lister ici les controlleurs possibles
                case 0x0:   
                    return new Zombie(this);
            }

            return null;
        }

        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.layer == 9 || c.gameObject.layer == 10 )  
            {
                isMoving = false;
                targetPosition = transform.position;
                Debug.Log("ok");
            }
        }


        public void SetPosition(Vector3 pos) {
            position = pos;
            transform.position = pos;
        }

        public abstract void Attack(AbstractUnit anotherUnit);

        public abstract void UpdateUnit();

        public abstract bool Kill();

        public Vector3 GetPosition() {
            return position;
        }
        

        protected void UpdateGameObject()
        {
            if (livingEntityCount > 0)
                rigidBody.MovePosition(position);
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
