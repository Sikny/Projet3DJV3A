using System.Collections.Generic;
using Game;
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
        [HideInInspector] public List<AbstractUnit> units = new List<AbstractUnit>();


        /* Données de l'ancien système nécessaire aux unités */
        public Camera cam;
        public LayerMask groundMask;
        public float rotationSpeed = 300f;
        public float speed = 5f;

        public Material defaultMaterial;
        public Material selectedMaterial;
        public const float YPos = 0.5f;

        public bool isRunning;

        public void SetRunning(bool run)
        {
            isRunning = run;
        }
        
        public void Start() {
            cam = FindObjectOfType<Camera>();
            UnitLibData.selectedUnit = null;

            /* On se servira de ça pour appeler les updates des units */
            UnitLibData.cam = cam;
            UnitLibData.speed = speed;
            UnitLibData.rotationSpeed = rotationSpeed;
            UnitLibData.groundMask = groundMask;
            UnitLibData.units = units; // Penser à update si 
            UnitLibData.deltaTime = 0;
        }

        public Transform SpawnUnit(EntityType unitType, AbstractUnit unit, Vector3 position) {
            var newUnit = Instantiate(unit, position, Quaternion.identity);
            newUnit.Init(unitType, entityDict.GetEntityType(unitType), sizeUnit);
            newUnit.SetPosition(position);
            units.Add(newUnit);
            UnitLibData.units = units;
            return newUnit.transform;
        }

        private bool CheckPlaceable()
        {
            Cursor cursor = TerrainGrid.Instance.cursor;
            
            foreach (var cell in cursor.cursorCells)
            {
                if (cell.posY > 0.1f || cell.posY <= -0.1f)
                    return false;
            }
            return true;
        }

        public List<AbstractUnit> GetUnits()
        {
            return units;
        }
        
        public void DoClick() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Inventory inventory = GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.LEVEL
                ? GameSingleton.Instance.GetPlayer().storyModeInventory
                : GameSingleton.Instance.GetPlayer().arcadeModeInventory;
            // Placement d'une unité de l'inventaire
            if (inventory.selectedStoreUnit != null
                && Physics.Raycast(ray, out hit, 100f, 1 << 8)) {
                Vector3 position = new Vector3(Mathf.Floor(hit.point.x)+0.5f, YPos,
                    Mathf.Floor(hit.point.z)+0.5f) ;

                bool isPlaceable = CheckPlaceable();
                if (isPlaceable)
                {
                    StoreUnit unit = inventory.selectedStoreUnit;
                    SpawnUnit(unit.entityType, playerUnitPrefab, position);
                    inventory.RemoveUnit(unit);
                    inventory.selectedStoreUnit = null;
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
                if (UnitLibData.selectedUnit != null)
                {
                    foreach (var entity in UnitLibData.selectedUnit.entities)
                    {
                        entity.entityRenderer.material = defaultMaterial;
                    }   
                }
                UnitLibData.selectedUnit = hit.transform.GetComponentInParent<PlayerUnit>();
                foreach (var entity in UnitLibData.selectedUnit.entities)
                {
                    entity.entityRenderer.material = selectedMaterial;
                }

                //UnitLibData.selectedUnit.entities
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
                int unitCount = units.Count;
                for (int i = 0; i < unitCount; i++)
                {
                    units[i].UpdateUnit();
                    if (units[i].GetNumberAlive() <= 0)
                    {
                        units[i].Kill();
                        units.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}