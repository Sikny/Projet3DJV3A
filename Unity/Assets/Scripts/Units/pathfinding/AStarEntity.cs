using DG.Tweening;
using Units.PathFinding;
using UnityEngine;

namespace Units.Pathfinding {
    public class AStarEntity : MonoBehaviour {
        private bool _movingToDest;
        public bool MoveTo(Vector3 destination, AStarHandler handler) {
            if (_movingToDest) return true;
            handler.UpdateTransform(transform, destination, 1.0f);
            Vector3 position = transform.position;
            position.y = destination.y;
            float distance = Vector3.Distance(destination, position);
            if (distance <= 0.75f) {
                _movingToDest = true;
                transform.DOMoveZ(destination.z, UnitLibData.speed).SetSpeedBased().SetEase(Ease.Linear);
                transform.DOMoveX(destination.x, UnitLibData.speed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => {
                    _movingToDest = false;
                });
                return true;
            }
            return false;
        }
    }
}
