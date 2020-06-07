﻿using System;
using Items;
using Units.utils;
using UnityEngine;
using Utility;

namespace Units {
    public abstract class AbstractUnit : MonoBehaviour {
        public int entityCount;
        protected Entity[] entities;
        protected Vector3 position;
        public Vector3 targetPosition;

        protected AbstractUnit _unitTarget;

        // The interaction controller (zombie, bowman, giant...)
        protected Controller brain;
        private int idBrain;
        private EntityNative native;
        private EntityType _entityType;
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


        //private EquipmentEffect[] _equipmentEffects = new EquipmentEffect[16];
        private Equipment _currentEquipment;
        protected bool initialized;
        
		public virtual bool Init(EntityType idType,Entity entityModel, int entityCountP) {
            brain = GetControllerFromId(idType);
            
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
        private Controller GetControllerFromId(EntityType id)
        {
            this.idBrain = (int) id;
            switch (id) 
            {
                // Lister ici les controlleurs possibles
                case EntityType.Soldier:
                    native = EntityNative.Soldier;
                    _entityType = EntityType.Soldier;
                    return new Soldier(this);
                case EntityType.Archer:
                    native = EntityNative.Archer;
                    _entityType = EntityType.Archer;
                    return new Archer(this);
                case EntityType.Mage:
                    native = EntityNative.Mage;
                    _entityType = EntityType.Mage;
                    return new Wizard(this);
                case EntityType.Spearman:
                    native = EntityNative.Soldier;
                    _entityType = EntityType.Spearman;
                    return new Spearman(this);
                case EntityType.Knight:
                    native = EntityNative.Soldier;
                    _entityType = EntityType.Knight;
                    return new Knight(this);
                case EntityType.WhiteKnight:
                    native = EntityNative.Soldier;
                    _entityType = EntityType.WhiteKnight;
                    return new WhiteKnight(this);
                case EntityType.Horseman:
                    native = EntityNative.Archer;
                    _entityType = EntityType.Horseman;
                    return new Horseman(this);
                case EntityType.Executionist:
                    native = EntityNative.Mage;
                    _entityType = EntityType.Executionist;
                    return new Executionist(this);
                case EntityType.WhiteMage:
                    native = EntityNative.Mage;
                    _entityType = EntityType.WhiteMage;
                    return new WhiteMage(this);
                case EntityType.BlackMage:
                    native = EntityNative.Mage;
                    _entityType = EntityType.BlackMage;
                    return new BlackMage(this);
                case EntityType.Demonist:
                    native = EntityNative.Mage;
                    _entityType = EntityType.Demonist;
                    return new Demonist(this);
                case EntityType.RedMage:
                    native = EntityNative.Mage;
                    _entityType = EntityType.RedMage;
                    return new RedMage(this);
                case EntityType.Bard:
                    native = EntityNative.Mage;
                    _entityType = EntityType.Bard;
                    return new Bard(this);
                case EntityType.Arbalist:
                    native = EntityNative.Archer;
                    _entityType = EntityType.Arbalist;
                    return new Arbalist(this);
                case EntityType.Hunter:
                    native = EntityNative.Archer;
                    _entityType = EntityType.Hunter;
                    return new Hunter(this);
                case EntityType.MachineArc:
                    native = EntityNative.Archer;
                    _entityType = EntityType.MachineArc;
                    return new MachineArc(this);
                case EntityType.Catapultist:
                    native = EntityNative.Archer;
                    _entityType = EntityType.Catapultist;
                    return new Catapultist(this);
                case EntityType.Sniper:
                    native = EntityNative.Archer;
                    _entityType = EntityType.Sniper;
                    return new Sniper(this);
                

            }

            return null;
        }

        protected static float GetEfficientCoef(AbstractUnit from, AbstractUnit to)
        {
            float[,] matrixEfficient = new float[,]
            {
                {1.0f, 1.25f, 0.75f},
                {0.75f, 1.0f, 1.25f},
                {1.25f, 0.75f, 1.0f}
            };

            return matrixEfficient[(int)from.native, (int)to.native];
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
            //if (livingEntityCount > 0)
            //    rigidBody.MovePosition(position);
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
        
        public void AddEffect(int idEffect, int level, float timeout)
        {
            effect[idEffect] = new Effect(idEffect, level, timeout);
        }
        
        public void AddEquipment(int idEffect, int level, Equipment equipment)
        {
            //effect[idEffect] = new Effect(idEffect, level);
            //add equipment to unit
            Debug.Log("current equip of unit: " + _currentEquipment);
            if(_currentEquipment.itemName != null)
                GameSingleton.Instance.inventory.AddItem(equipment); 
            _currentEquipment = equipment;
            //_equipmentEffects[idEffect] = new EquipmentEffect(idEffect, level);
        }


        public EntityType GetEntityType()
        {
            return _entityType;
        }

        protected void UpdateTimeoutEffects()
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

        public Effect GetEffect(int id)
        {
            return effect[id];
        }
        /*public EquipmentEffect GetEquipmentEffect(int id)
        {
            return _equipmentEffects[id];
        }*/
    }
    
}
