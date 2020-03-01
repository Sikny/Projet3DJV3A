using UnityEngine;

namespace Units {
    // group of units
    public class UnitGroupController : MonoBehaviour {
        public UnitController unitControllerPrefab;
        public int unitCount;
        public bool isRemote;
        
        [SerializeField] private UnitController[] units;
        private UnitController _selected;

        private Vector3 unitSpawnPosition;
        // Start is called before the first frame update
        void Start() {
            units = new UnitController[unitCount];
            Vector3 tPos = transform.position;
            var positions = PathFinderAstar.GetInstance()
                .GetAdjacent((int) tPos.z, (int) tPos.x);
            unitSpawnPosition = new Vector3(0,1,0);
            //unit.transform.SetParent(transform);
            for (int i = 0; i < units.Length; i++) {
                if (i > 8) return;
                UnitController unit = Instantiate(unitControllerPrefab, new Vector3(positions[i].x, 1f, positions[i].y), Quaternion.identity);
                unit.transform.SetParent(transform);
                units[i] = unit;
                unit.SetManager(this);
                //unitSpawnPosition.z += 0.5f;
            }
        }

        public void SetSelected(UnitController main) {
            _selected = main;
            if (!main.SetTargetPosition()) return;
            foreach (var t in units) {
                if (t != main && t != null) {
                    if(isRemote)
                        t.SetTargetPosition(main.targetPosition + t.transform.position - main.transform.position);
                    else {
                        t.SetTargetPosition(main.targetPosition + t.transform.position - main.transform.position);
                    }
                }
            }
        }
    }
}
