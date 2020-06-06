using System.Collections.Generic;
using Items;
using Terrain;
using UI;
using UnityEngine;
using Utility;
using Cursor = Terrain.Cursor;

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
            UnitLibData.selectedUnit = null;

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

        public bool CheckPlaceable()
        {
            Cursor cursor = TerrainGrid.Instance.cursor;
            foreach (var cell in cursor.cursorCells)
            {
                if (cell.posY > 1 || cell.posY <= -0.4)
                    return false;
            }

            return true;
        }
        public void DoClick() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
           
            // Placement d'une unité de l'inventaire
            if (GameSingleton.Instance.uiManager.inventory.selectedStoreUnit != null
                && Physics.Raycast(ray, out hit, 100f, 1 << 8)) {
                Vector3 position = new Vector3(Mathf.Floor(hit.point.x)+0.5f, YPos,
                    Mathf.Floor(hit.point.z)+0.5f) ;

                bool isPlaceable = CheckPlaceable();
                if (isPlaceable)
                {
                    StoreUnit unit = GameSingleton.Instance.uiManager.inventory.selectedStoreUnit;
                    SpawnUnit(unit.entityType, playerUnitPrefab, position);
                    GameSingleton.Instance.uiManager.inventory.RemoveUnit(unit);
                    GameSingleton.Instance.uiManager.inventory.selectedStoreUnit = null;
                }
                else
                {
                    Popups.instance.Popup("Unit is not placeable here, please try somewhere else", Color.red);
                }

            }
            
            // Fight start
            if (!isRunning) return;
            // Allied Unit selection
            if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
            {
                UnitLibData.selectedUnit = hit.transform.GetComponent<PlayerUnit>();
            }
            else if (UnitLibData.selectedUnit != null)
            {
                // Click on ground
                if (Physics.Raycast(ray, out hit, 100f, 1 << 8))
                {
                    UnitLibData.selectedUnit.SetTargetPosition(TerrainGrid.Instance.cursor.transform.position);
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