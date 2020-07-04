using Units.PathFinding;
using UnityEngine;

namespace Units.Pathfinding {
    public class AStarEntity : MonoBehaviour
    {
        public void MoveTo(Vector3 destination, AStarHandler handler) {
            handler.UpdateTransform(transform, destination, 1.0f);
        }
    }
}
