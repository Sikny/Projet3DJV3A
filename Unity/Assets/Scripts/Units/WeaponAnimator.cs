using UnityEngine;

namespace Units {
    public enum WeaponType {
        Sword, Bow, Staff
    }
    public class WeaponAnimator : MonoBehaviour {
        [SerializeField] private Animator animator;
        [SerializeField] private WeaponType weaponType;
        

        private void Awake() {
            animator.SetInteger("WeaponType", (int) weaponType);
        }
        
        public void Animate() {
            animator.SetTrigger("AttackTrigger");
        }
    }
}
