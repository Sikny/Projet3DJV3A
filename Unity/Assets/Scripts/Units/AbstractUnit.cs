using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Units {
    public abstract class AbstractUnit : MonoBehaviour {
        public int entityCount;
        protected Entity[] entities;
        protected Vector3 position;
        protected Vector3 targetPosition;

        protected AbstractUnit _unitTarget;

        // The interaction controller (zombie, bowman, giant...)
        protected Controller brain;
        private int idBrain;
        
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
		public Material circleMaterial;
		private Effect[] effect = new Effect[16]; // max
   	    private int nbEffectApplied = 0;

        protected bool initialized;
        
		public virtual bool Init(EntityType idType,Entity entityModel, int entityCountP) {
            brain = getControllerFromId(idType);
            
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
                        entityGo.circleRenderer.material = circleMaterial;
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

            for (int i = 0; i < effect.Length; i++)
            {
                effect[i].IdEffect = -1;
            }

            initialized = true;
            
            return true;
        }

        // Init of unit's controller
        private Controller getControllerFromId(EntityType id)
        {
            this.idBrain = (int) id;
            switch (id) 
            {
                // Lister ici les controlleurs possibles
                case EntityType.Soldier:   
                    return new Soldier(this);
                case EntityType.Archer:
                    return new Archer(this);
                case EntityType.Mage:
                    return new Wizard(this);
            }

            return null;
        }

        protected static float getEfficientCoef(AbstractUnit from, AbstractUnit to)
        {
            float[,] matrixEfficient = new float[,]
            {
                {1.0f, 1.25f, 0.75f},
                {0.75f, 1.0f, 1.25f},
                {1.25f, 0.75f, 1.0f}
            };

            return matrixEfficient[from.idBrain, to.idBrain];
        }

        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.layer == 9 || c.gameObject.layer == 10 )  
            {
                isMoving = false;
                targetPosition = transform.position;
            }
        }


        public void SetPosition(Vector3 pos) {
            position = pos;
            transform.position = pos;
        }

        public abstract void Attack(AbstractUnit anotherUnit, float damage);

        public abstract void UpdateUnit();

        public virtual void Kill() {
            Destroy(gameObject);
        }

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
        
        public void addEffect(int idEffect, int level, float timeout)
        {
            effect[idEffect] = new Effect(idEffect, level, timeout);
        }

        protected void updateTimeoutEffects()
        {
            for (int i = 0; i < effect.Length; i++)
            {
                if (effect[i].IdEffect != -1 && float.IsPositiveInfinity(effect[i].Timeout))
                {
                    effect[i].Timeout -= Time.deltaTime;
                    if (effect[i].Timeout <= 0)
                    {
                        effect[i].IdEffect = -1;
                    }
                }
            }
        }

        public Effect getEffect(int id)
        {
            return effect[id];
        }
    }
    
}
