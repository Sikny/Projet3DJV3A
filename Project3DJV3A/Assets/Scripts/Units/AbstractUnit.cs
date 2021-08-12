using System;
using Units.Controllers;
using Units.Controllers.Archer;
using Units.Controllers.Mage;
using Units.Controllers.Soldier;
using Units.utils;
using UnityEngine;

namespace Units {
    public abstract class AbstractUnit : MonoBehaviour {
        public int entityCount;
        public Entity[] entities;
        protected Vector3 position;
        public Vector3 targetPosition;
        protected Transform colliderObjectTransform;

        protected AbstractUnit unitTarget;

        // The interaction controller (zombie, bowman, giant...)
        public Controller brain;
        private EntityNative _native;
        private EntityType _entityType;

        public bool isMoving;

        /* Utile pour savoir si le leader doit être attrapé */
        protected int livingEntityCount;

		public Material circleMaterial;
		private Effect[] effect = new Effect[16]; // max


        //private EquipmentEffect[] _equipmentEffects = new EquipmentEffect[16];
        //private Equipment _currentEquipment;
        protected bool initialized;
        
		public virtual bool Init(EntityType idType,Entity entityModel, int entityCountP) {
            brain = GetControllerFromId(idType);
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
                        entityGo.circleRenderer.material = circleMaterial;
                        entityGo.transform.localPosition = new Vector3(i-entityScale.x*2,0,j-entityScale.z*2);
                        entities[counterInstance++] = entityGo;
                    }
                }
            }

            GameObject colliderObj = new GameObject("Collider");
            colliderObj.layer = gameObject.layer;
            colliderObj.transform.SetParent(transform);
            BoxCollider col = colliderObj.AddComponent<BoxCollider>();
            col.size = new Vector3(sqrtEntityCount, 1, sqrtEntityCount);
            colliderObjectTransform = colliderObj.transform;

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
            switch (id) 
            {
                // Lister ici les controllers possibles
                case EntityType.Soldier:
                    _native = EntityNative.Soldier;
                    _entityType = EntityType.Soldier;
                    return new Soldier(this);
                case EntityType.Archer:
                    _native = EntityNative.Archer;
                    _entityType = EntityType.Archer;
                    return new Archer(this);
                case EntityType.Mage:
                    _native = EntityNative.Mage;
                    _entityType = EntityType.Mage;
                    return new Wizard(this);
                case EntityType.Spearman:
                    _native = EntityNative.Soldier;
                    _entityType = EntityType.Spearman;
                    return new Spearman(this);
                case EntityType.Knight:
                    _native = EntityNative.Soldier;
                    _entityType = EntityType.Knight;
                    return new Knight(this);
                case EntityType.WhiteKnight:
                    _native = EntityNative.Soldier;
                    _entityType = EntityType.WhiteKnight;
                    return new WhiteKnight(this);
                case EntityType.Horseman:
                    _native = EntityNative.Soldier;
                    _entityType = EntityType.Horseman;
                    return new Horseman(this);
                case EntityType.Executionist:
                    _native = EntityNative.Mage;
                    _entityType = EntityType.Executionist;
                    return new Executionist(this);
                case EntityType.WhiteMage:
                    _native = EntityNative.Mage;
                    _entityType = EntityType.WhiteMage;
                    return new WhiteMage(this);
                case EntityType.BlackMage:
                    _native = EntityNative.Mage;
                    _entityType = EntityType.BlackMage;
                    return new BlackMage(this);
                case EntityType.Demonist:
                    _native = EntityNative.Mage;
                    _entityType = EntityType.Demonist;
                    return new Demonist(this);
                case EntityType.RedMage:
                    _native = EntityNative.Mage;
                    _entityType = EntityType.RedMage;
                    return new RedMage(this);
                case EntityType.Bard:
                    _native = EntityNative.Mage;
                    _entityType = EntityType.Bard;
                    return new Bard(this);
                case EntityType.Arbalist:
                    _native = EntityNative.Archer;
                    _entityType = EntityType.Arbalist;
                    return new Arbalist(this);
                case EntityType.Hunter:
                    _native = EntityNative.Archer;
                    _entityType = EntityType.Hunter;
                    return new Hunter(this);
                case EntityType.MachineArc:
                    _native = EntityNative.Archer;
                    _entityType = EntityType.MachineArc;
                    return new MachineArc(this);
                case EntityType.Catapultist:
                    _native = EntityNative.Archer;
                    _entityType = EntityType.Catapultist;
                    return new Catapultist(this);
                case EntityType.Sniper:
                    _native = EntityNative.Archer;
                    _entityType = EntityType.Sniper;
                    return new Sniper(this);
            }
            return null;
        }

        protected int GetEfficiencyType(float efficientCoef) {
            int res = 0;
            //display particule on anotherUnit (targeted unit) 
            if (Math.Abs(efficientCoef - 1f) < 0.001f) {
                return res;
                //attack is neutral gray
            }
            else if (efficientCoef < 1f) {
                res = -1;
                //attack is inefficient red 
            }
            else {
                res = 1;
                //attack is efficient green 
            }
            return res;
        }

        protected static float GetEfficientCoef(AbstractUnit from, AbstractUnit to) {
            var matrixEfficient = new float[3][];
            matrixEfficient[0] = new [] {1.0f, 1.25f, 0.75f};
            matrixEfficient[1] = new [] {0.75f, 1.0f, 1.25f};
            matrixEfficient[2] = new [] {1.25f, 0.75f, 1.0f};
            return matrixEfficient[(int)from._native][(int)to._native];
        }

        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.layer == 9 || c.gameObject.layer == 10)  
            {
                isMoving = false;
                targetPosition = transform.position;
            }
        }


        public void SetPosition(Vector3 pos) {
            position = pos;
            colliderObjectTransform.position = pos;
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
        
        /*public void AddEquipment(int idEffect, int level, Equipment equipment)
        {
            //effect[idEffect] = new Effect(idEffect, level);
            //add equipment to unit
            //Debug.Log("current equip of unit: " + _currentEquipment);
            //if(_currentEquipment.itemName != null)
              //  GameSingleton.Instance.inventory.AddItem(equipment); 
            _currentEquipment = equipment;
            //_equipmentEffects[idEffect] = new EquipmentEffect(idEffect, level);
        }*/
        
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
