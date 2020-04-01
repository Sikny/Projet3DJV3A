﻿using UnityEngine;

namespace Units {
    public class UnitsManager : MonoBehaviour
    {
        public UnitController unitController;
        public int unitCount;
        
        [SerializeField] private UnitController[] units;
        private UnitController _selected;

        private Vector3 unitSpawnPosition;
        // Start is called before the first frame update
        void Start()
        {
            units = new UnitController[9];
            var positions = PathFinderAstar.GetInstance()
                .GetAdjacent((int) transform.position.z, (int) transform.position.x);
            unitSpawnPosition = new Vector3(0,1,0);
            //unit.transform.SetParent(transform);
            for (int i = 0; i < units.Length; i++)
            {
                if (i > 8) return;
                UnitController unit = Instantiate(unitController, new Vector3(positions[i].x, 1f, positions[i].y), Quaternion.identity);
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
                if (t != main) {
                    t.SetTargetPosition(main.targetPosition + t.transform.position - main.transform.position);
                }
            }
        }
    }
}