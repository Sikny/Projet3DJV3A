using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;
using UnitSystem;
using UnityEngine;

namespace Units.proto
{

    public class SystemUnit : MonoBehaviour
    {
        /*
         * Ok ici sera mis toutes les unités instanciées (de type AI ou Remoted)
         */
        [NonSerialized]
        public List<AbstractUnit> units = new List<AbstractUnit>();

        [SerializeField] public GameObject entityModel;
        [SerializeField] public int sizeUnit = 9;
        
        public void Start()
        {
            //On fabrique nos 2 entités rivales
            AbstractUnit remoteUnit = new RemotedUnit(sizeUnit);
            AbstractUnit aiUnit = new AIUnit(sizeUnit);

            if (remoteUnit.init(entityModel) && aiUnit.init(entityModel))
            {
                // On les met dans la liste. On mettra que ces 2 là pour l'instant..
                units.Add(remoteUnit);
                units.Add(aiUnit);
            }
        }
    }
}