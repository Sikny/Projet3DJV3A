﻿using System.Collections;
using UnityEngine;

namespace Units {
    public class Entity : MonoBehaviour {
        private int _strength; // j'ai pas lu les specs mais ca doit etre une bonne idée
        private int _life;
        private int _maxLife;

        public MeshRenderer meshRenderer;
        public Transform fillBar;

        private Color _firstColor;

        private void Awake() {
            _strength = 5;
            _maxLife = 100;
            _life = _maxLife;
            _firstColor = meshRenderer.material.color;
        }

        public void InitColor(Color col) {
            meshRenderer.material.color = col;
            _firstColor = col;
        }

        public void ResetColor() {
            meshRenderer.material.color = _firstColor;
        }

        private void KillEntity() {
            Destroy(gameObject);
        }

        public int ChangeLife(int deltaValue) {
            _life += deltaValue;
            if (_life > _maxLife) _life = _maxLife;
            else if (_life < 0) _life = 0;
            if (deltaValue < 0) {
                StartCoroutine(Blink());
            }
            if (_life == 0) KillEntity();
            Vector3 scaleFillBar = fillBar.localScale;
            scaleFillBar.x = (float) _life / _maxLife;
            fillBar.localScale = scaleFillBar;
            return _life;
        }
        
        private readonly WaitForSeconds blinkTime = new WaitForSeconds(0.2f);
        private float blinkAlpha = 0.5f;
        private IEnumerator Blink() {
            var material = meshRenderer.material;
            Color meshCol = material.color;
            meshCol.a = blinkAlpha;
            material.color = meshCol;
            yield return blinkTime;
            meshCol.a = 1f;
            material.color = meshCol;
        }

        public int GetStrength() {
            return _strength;
        }
    }
}