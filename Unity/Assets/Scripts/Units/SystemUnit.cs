using System.Collections.Generic;
using Items;
using UnityEngine;
using Utility;

namespace Units {
    public class SystemUnit : MonoBehaviour {
        [SerializeField] private int sizeUnit = 9;

        public EntityDict entityDict;
        public PlayerUnit playerUnitPrefab;
        public AiUnit aiUnitPrefab;
        private List<AbstractUnit> _units = new List<AbstractUnit>();


        /** Données de l'ancien système nécessaire aux unités*/
        public Camera cam;
        public LayerMask groundMask;
        public float rotationSpeed = 300f;
        public float speed = 5f;
        
        public const float YPos = 0.5f;

        public bool isRunning;

        public void SetRunning(bool run)
        {
            isRunning = run;
        }
        
        public void Start() {
            UnitLibData._selectedUnit = null;

            /* On se servira de ça pour appeler les updates des units */
            UnitLibData.cam = cam;
            UnitLibData.speed = speed;
            UnitLibData.rotationSpeed = rotationSpeed;
            UnitLibData.groundMask = groundMask;
            UnitLibData.units = _units; // Penser à update si 
            UnitLibData.deltaTime = 0;
        }

        public Transform SpawnUnit(EntityType unitType, AbstractUnit unit, Vector3 position) {
            var newUnit = Instantiate(unit);
            newUnit.SetPosition(position);
            newUnit.Init(unitType, entityDict.GetEntityType(unitType), sizeUnit);
            _units.Add(newUnit);
            UnitLibData.units = _units;
            return newUnit.transform;
        }

        public void DoClick() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (GameSingleton.Instance.uiManager.inventory.selectedStoreUnit != null
                && Physics.Raycast(ray, out hit, 100f, 1 << 8)) {
                Vector3 position = new Vector3(Mathf.Floor(hit.point.x)-0.5f, YPos,
                    Mathf.Floor(hit.point.z)-0.5f) ;
                StoreUnit unit = GameSingleton.Instance.uiManager.inventory.selectedStoreUnit;
                SpawnUnit(unit.entityType, playerUnitPrefab, position);
                GameSingleton.Instance.uiManager.inventory.RemoveUnit(unit);
                GameSingleton.Instance.uiManager.inventory.selectedStoreUnit = null;
            }
            if (!isRunning) return;
            if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
            {
                UnitLibData._selectedUnit = hit.transform.GetComponent<PlayerUnit>();
            }
            else if (UnitLibData._selectedUnit != null)
            {
                if (Physics.Raycast(ray, out hit, 100f, 1 << 8))
                {
                    UnitLibData._selectedUnit.SetTargetPosition(YPos);
                }
            }
        }

        public void Update() {
            if (isRunning)
            {
                UnitLibData.deltaTime = Time.deltaTime;
                int unitCount = _units.Count;
                for (int i = 0; i < unitCount; i++)
                {
                    _units[i].UpdateUnit();
                    if (_units[i].GetNumberAlive() <= 0)
                    {
                        _units[i].Kill();
                        _units.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}