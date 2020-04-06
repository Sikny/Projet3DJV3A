using System.Collections.Generic;
using System;
using Items;
using TerrainGeneration;
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
        
        private const int YPos = 1;

        public bool isRunning = false;

        public void setRunning(bool run)
        {
            isRunning = run;
        }
        
        public void Start() {
            UnitLibData._selectedUnit = null;
            /*Vector3[] playerUnitsPositions = {
                new Vector3(3, YPos, 5),
                new Vector3(7, YPos, 3)
            };*/
            
            /*Vector3[] aiUnitsPositions = {
                new Vector3(1, YPos, 1),
                new Vector3(10, YPos, 8)
            };
            numberAi = aiUnitsPositions.Length;*/

            int i = 0;
            
            /*foreach (var unitPos in playerUnitsPositions) {
                PlayerUnit unit = Instantiate(playerUnitPrefab);
                unit.SetPosition(unitPos);
                unit.Init(EntityType.Soldier, entityDict.GetEntityType(EntityType.Soldier), sizeUnit);    // Archer
                _units.Add(unit);
            }*/

            /*foreach (var unitPos in aiUnitsPositions) {
                AiUnit unit = Instantiate(aiUnitPrefab);
                unit.SetPosition(unitPos);
                unit.Init(EntityType.Archer, entityDict.GetEntityType(EntityType.Archer), sizeUnit);    // Zombie ?
                _units.Add(unit);
            }*/

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

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (InventoryContent.instance.selectedStoreUnit != null
                    && Physics.Raycast(ray, out hit, 100f, 1 << 8)) {
                    Vector3 position = new Vector3(Mathf.Floor(hit.transform.position.x)-0.5f, 1,
                        Mathf.Floor(hit.transform.position.z)-0.5f) ;
                    StoreUnit unit = InventoryContent.instance.selectedStoreUnit;
                    print(unit.entityType);
                    SpawnUnit(unit.entityType, playerUnitPrefab, position);
                    InventoryContent.instance.RemoveUnit(unit);
                    InventoryContent.instance.selectedStoreUnit = null;
                }
                if (!isRunning) return;
                if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
                {
                    if (UnitLibData._selectedUnit != null)
                        UnitLibData._selectedUnit.Deselect();
                    UnitLibData._selectedUnit = hit.transform.GetComponent<PlayerUnit>();
                    UnitLibData._selectedUnit.Select();
                }
                else if (UnitLibData._selectedUnit != null)
                {
                    if (Physics.Raycast(ray, out hit, 100f, 1 << 8))
                    {
                        UnitLibData._selectedUnit.SetTargetPosition();
                    }
                }
            }
            
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