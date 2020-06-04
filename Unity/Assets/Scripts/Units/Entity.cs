using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Units {
    public class Entity : MonoBehaviour {
        private int _strength; // j'ai pas lu les specs mais ca doit etre une bonne idée
        private int _life;
        private int _maxLife;

        public MeshRenderer meshRenderer;
        public RectTransform fillBar;
        public MeshRenderer circleRenderer;

        public GameObject hitParticles;
        public GameObject effectiveHitParticles;
        public GameObject notEffectiveHitParticles;

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
        
        private void KillEntity() {
            Destroy(gameObject);
        }

        public int ChangeLife(int deltaValue, int efficiencyType) {
            
            
            _life += deltaValue;
            if (_life > _maxLife) _life = _maxLife;
            else if (_life < 0) _life = 0;
            if (deltaValue < 0)
            {
                Debug.Log("efficient type is : " + efficiencyType);
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
            hitParticles.layer = 0;
            yield return _blinkTime;
            hitParticles.layer = 31;
        }
        private IEnumerator BlinkEfficient() {
            effectiveHitParticles.layer = 0;
            yield return _blinkTime;
            effectiveHitParticles.layer = 31;
        }
        private IEnumerator Blink() {
            notEffectiveHitParticles.layer = 0;
            yield return _blinkTime;
            notEffectiveHitParticles.layer = 31;
        }

        public int GetStrength() {
            return _strength;
        }
        
    }
}
