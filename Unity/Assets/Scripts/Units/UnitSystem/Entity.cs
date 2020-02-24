using UnityEngine;

namespace Units.UnitSystem {
    public class Entity : MonoBehaviour {
        private int _strength; // j'ai pas lu les specs mais ca doit etre une bonne idée
        private int _life;

        public MeshRenderer meshRenderer;

        private Color _firstColor;

        private void Awake() {
            _strength = 5;
            _life = 100;
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
            if (_life > 100) _life = 100;
            else if (_life < 0) _life = 0;
            if (_life == 0) KillEntity();
            return _life;
        }

        public int GetStrength() {
            return _strength;
        }
    }
}
