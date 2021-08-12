using DG.Tweening;
using UnityEngine;

namespace Units {
    public enum WeaponType {
        Sword, Bow, Staff
    }
    public class WeaponAnimator : MonoBehaviour {
        [SerializeField] private Animator animator;
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private Transform projectile;
        private Transform _target;

        private Vector3 _projectilePos;
        private Quaternion _projectileRot;
        

        private void Awake() {
            animator.SetInteger("WeaponType", (int) weaponType);
            if (projectile != null)
            {
                _projectilePos = projectile.localPosition;
                _projectileRot = projectile.localRotation;
            }
        }
        
        public void Animate() {
            animator.SetTrigger("AttackTrigger");
        }

        public void RangeShot()
        {
            if (projectile != null && _target != null)
            {
                projectile.LookAt(_target);
                projectile.DOMove(_target.position, 10f).SetSpeedBased().SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        projectile.localPosition = _projectilePos;
                        projectile.localRotation = _projectileRot;
                    });
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}
