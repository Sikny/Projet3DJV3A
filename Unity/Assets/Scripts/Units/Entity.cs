using System.Collections;
using Units.Pathfinding;
using UnityEngine;
using Utility;

namespace Units {
    public class Entity : MonoBehaviour {
        private int _strength; // j'ai pas lu les specs mais ca doit etre une bonne idée
        private int _life;
        private int _maxLife;

        public RectTransform fillBar;
        public MeshRenderer circleRenderer;
        public MeshRenderer entityRenderer;
        public GameObject hitParticles;
        public GameObject effectiveHitParticles;
        public GameObject notEffectiveHitParticles;

        public WeaponAnimator weaponAnimator;
        [HideInInspector]
        public bool isDead;
        public AStarEntity aStarEntity;
        
        private void Awake() {
            _strength = 5;
            _maxLife = 100;
            _life = _maxLife;
        }
        
        public void ResetLife()
        {
            _life = 100;
        }

        public int GetLife()
        {
            return _life;
        }

        public int GetMaxLife()
        {
            return _maxLife;
        }
        private void KillEntity()
        {
            isDead = true;
            Destroy(gameObject);
        }

        public int Attack(Entity target, int deltaValue, int efficiencyType) {
            Vector3 targetPos = target.transform.position; 
            transform.LookAt(new Vector3(targetPos.x, transform.position.y, 
                targetPos.z));
            transform.Rotate(0, -90f, 0);    // entity Z axis is not on world Z axis
            if (weaponAnimator != null)
            {
                weaponAnimator.Animate();
                weaponAnimator.SetTarget(target.transform);
                weaponAnimator.RangeShot();    // activates only on range units
            }
            return target.ChangeLife(deltaValue, efficiencyType);
        }

        private int ChangeLife(int deltaValue, int efficiencyType) {
            _life += deltaValue;
            if (_life > _maxLife) _life = _maxLife;
            else if (_life < 0) _life = 0;
            if (deltaValue < 0)
            {
                if (efficiencyType == -1)
                    StartCoroutine(BlinkInefficient());
                else if (efficiencyType == +1)
                    StartCoroutine(BlinkEfficient());
                else 
                    StartCoroutine(Blink());
            }
            if (_life == 0) KillEntity();
            Vector3 scaleFillBar = fillBar.localScale;
            scaleFillBar.x = (float) _life / _maxLife;
            fillBar.localScale = scaleFillBar;
            return _life;
        }
        
        private readonly WaitForSeconds _blinkTime = new WaitForSeconds(0.2f);
        
        private IEnumerator BlinkInefficient() {
            notEffectiveHitParticles.layer = 0;
            yield return _blinkTime;
            notEffectiveHitParticles.layer = 31;
        }
        private IEnumerator BlinkEfficient() {
            effectiveHitParticles.layer = 0;
            yield return _blinkTime;
            effectiveHitParticles.layer = 31;
        }
        private IEnumerator Blink() {
            hitParticles.layer = 0;
            yield return _blinkTime;
            hitParticles.layer = 31;
        }

        public int GetStrength() {
            return _strength;
        }
    }
}
