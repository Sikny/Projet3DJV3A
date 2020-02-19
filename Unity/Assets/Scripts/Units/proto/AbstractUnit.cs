using System.Collections;
using System.Collections.Generic;
using Units.proto;
using UnityEngine;

namespace UnitSystem
{
    public abstract class AbstractUnit
    {
        private int numberEntity;
        protected Entity[] entity;
        protected Vector3 position;
        protected Quaternion rotation; //todo

        protected Vector3 targetPosition;
        protected Vector3 lookAtTarget;
        
        protected bool isMoving = false;
        protected bool isTurning = false;
        
        public AbstractUnit(int numberEntity, Vector3 position)
        {
            this.numberEntity = numberEntity;
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

        public abstract void update();

        public abstract bool kill();
    }
}
