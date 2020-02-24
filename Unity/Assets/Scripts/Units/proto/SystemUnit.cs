using System;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Units.proto {
    public class SystemUnit : MonoBehaviour {
        /*
         * Ok ici sera mis toutes les unités instanciées (de type AI ou Remoted)
         */
        [NonSerialized] private readonly List<AbstractUnit> units = new List<AbstractUnit>();
        [SerializeField] private Entity entityModel;
        [SerializeField] private int sizeUnit = 9;

        private RemotedUnit _selectedUnit;

        /** Données de l'ancien système nécessaire aux unités*/
        public Camera cam;
        public LayerMask groundMask;
        public Interactable focus;
        public float rotationSpeed = 300f;
        public float speed = 5f;

        private int numberAi = 1;
        private int numberRemote = 1;

        private const int YPos = 1;

        public void Start() {
            _selectedUnit = null;
            AbstractUnit[] playerUnits = {
                new RemotedUnit(sizeUnit, new Vector3(3, YPos, 5)),
                new RemotedUnit(sizeUnit, new Vector3(5, YPos, 3))
            };
            AbstractUnit[] aiUnits = {
                new AIUnit(sizeUnit, new Vector3(1, YPos, 1))
            };

            foreach (var unit in playerUnits) {
                unit.init(entityModel);
                units.Add(unit);
            }

            foreach (var unit in aiUnits) {
                unit.init(entityModel);
                units.Add(unit);
            }

            /* On se servira de ça pour appeler les updates des units */
            UnitLibData.cam = cam;
            UnitLibData.focus = focus;
            UnitLibData.speed = speed;
            UnitLibData.groundMask = groundMask;
            UnitLibData.units = units; // Penser à update si 
            UnitLibData.deltaTime = 0;
        }

        public void Update() {
            UnitLibData.deltaTime = Time.deltaTime;
            
            foreach (var unit in units) {
                unit.update();
                if (unit.getNumberAlive() <= 0) {
                    if (unit is AIUnit) numberAi--;
                    else if (unit is RemotedUnit) numberRemote--;
                    unit.kill();
                    units.Remove(unit); // a revoir
                }
            }

            if (numberRemote == 0) {
                EndGameManager.typeEndGame = 0;
                SceneManager.LoadScene(2);
            }
            else if (numberAi == 0) {
                EndGameManager.typeEndGame = 1;
                SceneManager.LoadScene(2);
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                if (_selectedUnit != null)
                    _selectedUnit.SetTargetPosition();
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