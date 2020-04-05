using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Units {
    public class SystemUnit : MonoBehaviour {
        [SerializeField] private Entity entityModel;
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

        [SerializeField] private int numberAi = 1;
        [SerializeField] private int numberRemote = 1;

        private const int YPos = 1;

        public void Start() {
            UnitLibData._selectedUnit = null;
            Vector3[] playerUnitsPositions = {
                new Vector3(3, YPos, 5),
                new Vector3(7, YPos, 3)
            };
            numberRemote = playerUnitsPositions.Length;
            Vector3[] aiUnitsPositions = {
                new Vector3(1, YPos, 1),
                new Vector3(10, YPos, 8)
            };
            numberAi = aiUnitsPositions.Length;

            int i = 0;
            
            foreach (var unitPos in playerUnitsPositions) {
                PlayerUnit unit = Instantiate(playerUnitPrefab);
                unit.SetPosition(unitPos);
                unit.Init(EntityType.Soldier, entityDict.GetEntityType(EntityType.Soldier), sizeUnit);    // Archer
                _units.Add(unit);
            }

            foreach (var unitPos in aiUnitsPositions) {
                AiUnit unit = Instantiate(aiUnitPrefab);
                unit.SetPosition(unitPos);
                unit.Init(EntityType.Archer, entityDict.GetEntityType(EntityType.Archer), sizeUnit);    // Zombie ?
                _units.Add(unit);
            }

            /* On se servira de ça pour appeler les updates des units */
            UnitLibData.cam = cam;
            UnitLibData.speed = speed;
            UnitLibData.rotationSpeed = rotationSpeed;
            UnitLibData.groundMask = groundMask;
            UnitLibData.units = _units; // Penser à update si 
            UnitLibData.deltaTime = 0;
        }

        public void SpawnUnit(EntityType unitType, AbstractUnit unit, Vector3 position) {
            AbstractUnit newUnit = Instantiate(unit);
            newUnit.SetPosition(position);
            newUnit.Init(unitType, entityDict.GetEntityType(unitType), sizeUnit);
            _units.Add(unit);
        }

        public void Update() {
            UnitLibData.deltaTime = Time.deltaTime;
            int unitCount = _units.Count;
            for (int i = 0; i < unitCount; i++) {
                if(_units[i] == null) continue;
                
                _units[i].UpdateUnit();
                if (_units[i].GetNumberAlive() <= 0) {
                    if (_units[i] is AiUnit) numberAi--;
                    else if (_units[i] is PlayerUnit) numberRemote--;
                    _units[i].Kill();
                    _units[i] = null;
                }
            }

            if (numberRemote == 0) {
                GameSingleton.Instance.EndGame(0);
            }
            else if (numberAi == 0) {
                GameSingleton.Instance.EndGame(1);
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, 1 << 9)) {
                    if(UnitLibData._selectedUnit != null)
                        UnitLibData._selectedUnit.Deselect();
                    UnitLibData._selectedUnit = hit.transform.GetComponent<PlayerUnit>();
                    UnitLibData._selectedUnit.Select();
                } else if (UnitLibData._selectedUnit != null) {
                    if (Physics.Raycast(ray, out hit, 100f, 1 << 8)) {
                        UnitLibData._selectedUnit.SetTargetPosition();
                    }
                }
            }
        }
    }
}