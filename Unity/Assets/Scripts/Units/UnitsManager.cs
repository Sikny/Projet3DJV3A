using UnityEngine;

namespace Units {
    public class UnitsManager : MonoBehaviour
    {
        public UnitController unitController;
        public int unitCount;
        
        private UnitController[] units;
        private UnitController _selected;

        private Vector3 unitSpawnPosition;
        // Start is called before the first frame update
        void Start()
        {
            units = new UnitController[unitCount];
            unitSpawnPosition = new Vector3(0,1,0);
            //unit.transform.SetParent(transform);
            for (int i = 0; i < units.Length; i++)
            {
                UnitController unit = Instantiate(unitController, unitSpawnPosition, Quaternion.identity);
                units[i] = unit;
                unit.SetManager(this);
                unitSpawnPosition.z += 0.5f;
            }
        }

        public void SetSelected(UnitController main) {
            _selected = main;
            main.SetTargetPosition();
            for (int i = 0; i < units.Length; i++) {
                if(units[i] != main)
                    units[i].SetTargetPosition(main.targetPosition + units[i].transform.position - main.transform.position);
            }
        }
    }
}
