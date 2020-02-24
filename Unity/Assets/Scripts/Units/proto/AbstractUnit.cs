using Units.proto;
using UnityEngine;

namespace UnitSystem {
    public abstract class AbstractUnit {
        protected int numberEntity;
        protected Entity[] entities;
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
            this.entities = new Entity[numberEntity];
            this.position = position;
            this.rotation = Quaternion.identity;
        }

        public virtual bool init(Entity entityModel) // pas très propre mais bon...
        {
            int counterInstance = 0;
            Entity leader = null;
            for (int i = 0; i < Mathf.Sqrt(numberEntity); i++)
            {
                for (int j = 0; j < Mathf.Sqrt(numberEntity); j++)
                {
                    if (counterInstance < numberEntity)
                    {
                        Entity entityGo;
                        if (leader == null)
                        {
                            // le premier sera le leader et le parent des n-1 autres
                            leader = Object.Instantiate(entityModel,
                                new Vector3(position.x+0.5f, position.y, position.z+0.5f), Quaternion.identity);
                            entityGo = leader;
                        }
                        else
                        {
                            entityGo = Object.Instantiate(entityModel,
                                new Vector3(i,0,j), Quaternion.identity);
                            entityGo.transform.SetParent(leader.transform);
                            entityGo.SetParent(this);
                            entityGo.transform.localPosition = new Vector3(i*2f,0,j*2f);
                        }
                        entities[counterInstance++] = entityGo;
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
                entities[0].transform.position = position;
        }

        public Entity getEntity(int index)
        {
            return entities[index];
        }

        public void popEntity(int index)
        {
            numberAlive--;
            entities[index] = null;
        }

        public int getNumberAlive()
        {
            return numberAlive;
        }
    }
    
}
