using Units.proto;
using UnityEngine;

namespace UnitSystem {
    public abstract class AbstractUnit
    {
        protected int numberEntity;
        protected Entity[] entity;
        protected Vector3 position;
        protected Quaternion rotation; //todo

        protected Vector3 targetPosition;
        protected Vector3 lookAtTarget;
        
        protected bool isMoving;
        protected bool isTurning;
        /**
         * Utile pour savoir si le leader doit être attrapé
         */
        protected int numberAlive;

        // On peu imaginer que les ennemis vont moins vite
        protected float speedEntity;
        
        public AbstractUnit(int numberEntity, Vector3 position)
        {
            this.numberEntity = numberEntity;
            this.numberAlive = numberEntity;
            this.entity = new Entity[numberEntity];
            this.position = position;
            this.rotation = Quaternion.identity;
        }

        public virtual bool init(GameObject entityModel) // pas très propre mais bon...
        {
            int counterInstance = 0;
            GameObject leader = null;
            for (int i = 0; i < Mathf.Sqrt(numberEntity); i++)
            {
                for (int j = 0; j < Mathf.Sqrt(numberEntity); j++)
                {
                    if (counterInstance < numberEntity)
                    {
                        GameObject entityGO = null;
                        if (leader == null)
                        {
                            // le premier sera le leader et le parent des n-1 autres
                            leader = (GameObject) Object.Instantiate(entityModel,
                                new Vector3(position.x+0.5f, position.y, position.z+0.5f), Quaternion.identity);
                            entityGO = leader;
                        }
                        else
                        {
                            entityGO = (GameObject) Object.Instantiate(entityModel,
                                new Vector3(i,0,j), Quaternion.identity);
                            entityGO.transform.parent = leader.transform;
                            entityGO.transform.localPosition = new Vector3(i*2f,0,j*2f);
                        }
                        entity[counterInstance++] = new Entity(entityGO);
                    }
                }
            }

            return true;
        }

        protected abstract void attack(AbstractUnit anotherUnit);

        public abstract void update();

        public abstract bool kill();

        public Vector3 getPosition()
        {
            return position;
        }
        
        protected virtual void Move()
        {
            if (targetPosition == null) return;
            position = Vector3.MoveTowards(position, targetPosition, UnitLibData.speed * Time.deltaTime * speedEntity);
            
            if (position == targetPosition) {
                isMoving = false;
            }
        }

        protected void updateGameobject()
        {
            if(numberAlive > 0)
                entity[0].associedGameObject.transform.position = position;
        }

        public Entity getEntity(int index)
        {
            return entity[index];
        }

        public void popEntity(int index)
        {
            numberAlive--;
            entity[index] = null;
        }

        public int getNumberAlive()
        {
            return numberAlive;
        }
    }
    
}
