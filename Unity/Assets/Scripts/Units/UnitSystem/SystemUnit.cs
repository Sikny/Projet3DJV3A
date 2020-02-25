using System.Collections.Generic;
using Units.proto;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Units.UnitSystem {
    public class SystemUnit : MonoBehaviour {
        [SerializeField] private Entity entityModel;
        [SerializeField] private int sizeUnit = 9;
        
        private readonly List<AbstractUnit> _units = new List<AbstractUnit>();

        private RemotedUnit _selectedUnit;

        /** Données de l'ancien système nécessaire aux unités*/
        public Camera cam;
        public LayerMask groundMask;
        public Interactable focus;
        public float rotationSpeed = 300f;
        public float speed = 5f;

        private int _numberAi;
        private int _numberRemote;

        private const int YPos = 1;

        public void Start() {
            _selectedUnit = null;
            Vector3[] playerUnitsPositions = {
                new Vector3(3, YPos, 5),
                new Vector3(5, YPos, 3)
            };
            _numberRemote = playerUnitsPositions.Length;
            Vector3[] aiUnitsPositions = {
                new Vector3(1, YPos, 1)
            };
            _numberAi = aiUnitsPositions.Length;

            foreach (var unitPos in playerUnitsPositions) {
                RemotedUnit unit = new GameObject("Allied Unit").AddComponent<RemotedUnit>();
                unit.SetPosition(unitPos);
                unit.transform.position = unitPos;
                unit.Init(entityModel, sizeUnit);
                _units.Add(unit);
            }

            foreach (var unitPos in aiUnitsPositions) {
                AiUnit unit = new GameObject("Enemy Unit").AddComponent<AiUnit>();
                unit.SetPosition(unitPos);
                unit.Init(entityModel, sizeUnit);
                _units.Add(unit);
            }

            /* On se servira de ça pour appeler les updates des units */
            UnitLibData.cam = cam;
            UnitLibData.focus = focus;
            UnitLibData.speed = speed;
            UnitLibData.groundMask = groundMask;
            UnitLibData.units = _units; // Penser à update si 
            UnitLibData.deltaTime = 0;
        }

        public void Update() {
            UnitLibData.deltaTime = Time.deltaTime;
            
            foreach (var unit in _units) {
                unit.UpdateUnit();
                if (unit.GetNumberAlive() <= 0) {
                    if (unit is AiUnit) _numberAi--;
                    else if (unit is RemotedUnit) _numberRemote--;
                    unit.Kill();
                    _units.Remove(unit);
                }
            }

            if (_numberRemote == 0) {
                EndGameManager.typeEndGame = 0;
                SceneManager.LoadScene(2);
            }
            else if (_numberAi == 0) {
                EndGameManager.typeEndGame = 1;
                SceneManager.LoadScene(2);
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, 1 << 9)) {
                    if(_selectedUnit != null)
                        _selectedUnit.Deselect();
                    _selectedUnit = hit.transform.GetComponent<RemotedUnit>();
                    _selectedUnit.Select();
                } else if (_selectedUnit != null) {
                    if (Physics.Raycast(ray, out hit, 100f, 1 << 8)) {
                        _selectedUnit.SetTargetPosition();
                    }
                }
            }
        }

        public void SelectUnit(RemotedUnit unit) {
            if (_selectedUnit != null) {
                _selectedUnit.Deselect();
            }
            _selectedUnit = unit;
            if (_selectedUnit != null) {
                _selectedUnit.Select();
            }
        }
    }
}