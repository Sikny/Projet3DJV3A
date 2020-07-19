using Units.utils;
using UnityEngine;

namespace Units.Controllers
{
    public abstract class Controller
    {

        protected AbstractUnit body;
        protected float basisAttack;
        protected float basisDefense;
        protected float basisSpeed;
        public float MultiplierScore { get; set; }

        protected float deltaTime;
        protected Controller(AbstractUnit body)
        {
            MultiplierScore = 1.0f;
            this.body = body;
            deltaTime = 0.0f;
        }

        public abstract bool Interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget);

        protected float GetVitessUnit()
        {
            float baseVitess = basisSpeed;

            Effect effect = body.GetEffect(0);
            //EquipmentEffect equipmentEffect = body.GetEquipmentEffect(0);
            int bonusLevel = effect.IdEffect == -1 ? 0 : effect.LevelEffect;
           // int bonusEquipmentLevel = equipmentEffect.IdEffect == -1 ? 0 : equipmentEffect.LevelEffect;

           return baseVitess + bonusLevel * 0.5f;
        }
        protected float GetAttackUnit(AbstractUnit target)
        {

            Effect effect = target.GetEffect(1); //defense
            //EquipmentEffect equipmentEffect = body.GetEquipmentEffect(1);

            int bonusLevel = effect.IdEffect == -1 ? 1 : effect.LevelEffect + 1; 
            //int bonusEquipmentLevel = equipmentEffect.IdEffect == -1 ? 1 : equipmentEffect.LevelEffect;


            return basisAttack/(bonusLevel*target.brain.basisDefense);// + bonusEquipmentLevel;
        }

        protected Entity GetFirstLivingEntity() {
            int ind = 0;
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if (body.entities[ind] != null) return body.entities[ind];
                    ind++;
                }
            }
            return null;
        }

        public bool positionLocked;
        protected void LockPosition(Vector3 position) {
            positionLocked = true;
            body.targetPosition = position;
        }

        public void UnlockPosition() {
            positionLocked = false;
        }
    }
}
