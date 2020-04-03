using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Utility;

namespace Units {
    public class SystemUnit : MonoBehaviour {
        [SerializeField] private Entity entityModel;
        [SerializeField] private int sizeUnit = 9;

        public PlayerUnit playerUnitPrefab;
        public AiUnit aiUnitPrefab;
        private AbstractUnit[] _units = new AbstractUnit[16];

        private PlayerUnit _selectedUnit;

        /** Données de l'ancien système nécessaire aux unités*/
        public Camera cam;
        public LayerMask groundMask;
        public float rotationSpeed = 300f;
        public float speed = 5f;

        [SerializeField] private int numberAi = 1;
        [SerializeField] private int numberRemote = 1;

        private const int YPos = 1;

        public void Start() {
            _selectedUnit = null;
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
                PlayerUnit unit = new GameObject("Allied Unit").AddComponent<PlayerUnit>();
                unit.SetPosition(unitPos);
                unit.transform.position = unitPos;
                unit.Init(0, entityModel, sizeUnit);
                _units[i++] = unit;
            }

            foreach (var unitPos in aiUnitsPositions) {
                AiUnit unit = new GameObject("Enemy Unit").AddComponent<AiUnit>();
                unit.SetPosition(unitPos);
                unit.Init(0, entityModel, sizeUnit);
                _units[i++] = unit;
            }

            /* On se servira de ça pour appeler les updates des units */
            UnitLibData.cam = cam;
            UnitLibData.speed = speed;
            UnitLibData.rotationSpeed = rotationSpeed;
            UnitLibData.groundMask = groundMask;
            UnitLibData.units = _units; // Penser à update si 
            UnitLibData.deltaTime = 0;
        }

        public void Update()
        {
            UnitLibData.deltaTime = Time.deltaTime;
            for (int i = 0; i < _units.Length; i++)
            {
                if (_units[i] == null) continue;

                _units[i].UpdateUnit();
                if (_units[i].GetNumberAlive() <= 0)
                {
                    if (_units[i] is AiUnit) numberAi--;
                    else if (_units[i] is PlayerUnit) numberRemote--;
                    _units[i].Kill();
                    _units[i] = null;
                }
            }

            if (numberRemote == 0)
            {
                GameSingleton.Instance.EndGame(0);
            }
            else if (numberAi == 0)
            {
                GameSingleton.Instance.EndGame(1);
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, 1 << 9)) {
                    if(_selectedUnit != null)
                        _selectedUnit.Deselect();
                    _selectedUnit = hit.transform.GetComponent<PlayerUnit>();
                    _selectedUnit.Select();
                } else if (_selectedUnit != null) {
                    if (Physics.Raycast(ray, out hit, 100f, 1 << 8)) {
                        _selectedUnit.SetTargetPosition();

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
                    {
                        if (_selectedUnit != null)
                            _selectedUnit.Deselect();
                        _selectedUnit = hit.transform.GetComponent<RemotedUnit>();
                        _selectedUnit.Select();
                    }
                    else if (_selectedUnit != null)
                    {
                        if (Physics.Raycast(ray, out hit, 100f, 1 << 8))
                        {
                            _selectedUnit.SetTargetPosition();
                        }
                    }
                }
            }


        }
    }
}
    