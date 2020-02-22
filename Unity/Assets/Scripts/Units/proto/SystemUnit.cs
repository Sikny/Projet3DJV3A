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
        [NonSerialized]
        public List<AbstractUnit> units = new List<AbstractUnit>();
        
        [SerializeField] public GameObject entityModel;
        [SerializeField] public int sizeUnit = 9;

        /** Données de l'ancien système nécessaire aux unités*/
        public Camera cam;
   
        public LayerMask groundMask;

        public Interactable focus;
   
        public float rotationSpeed = 300f;
        public float speed = 5f;

        public MeshRenderer renderer;

        private int numberAI = 1;
        private int numberRemote = 1;
        
        public void Start() {
            //On fabrique nos 2 entités rivales
            AbstractUnit remoteUnit = new RemotedUnit(sizeUnit);
            AbstractUnit aiUnit = new AIUnit(sizeUnit);

            if (remoteUnit.init(entityModel) && aiUnit.init(entityModel)) {
                // On les met dans la liste. On mettra que ces 2 là pour l'instant..
                units.Add(remoteUnit);
                units.Add(aiUnit);
            }

            /** On se servira de ça pour appeler les updates des units*/
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
                if (unit.getNumberAlive() <= 0)
                {
                    if (unit is AIUnit) numberAI--;
                    else if (unit is RemotedUnit) numberRemote--;
                    unit.kill();
                    units.Remove(unit);
                }
            }

            if (numberRemote == 0) {
                EndGameManager.typeEndGame = 0;
                SceneManager.LoadScene(2);
            }
            else if (numberAI == 0) {
                EndGameManager.typeEndGame = 1;
                SceneManager.LoadScene(2);
            }
        }
    }
}