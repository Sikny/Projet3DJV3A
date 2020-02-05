using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateUnits : MonoBehaviour
{
    public UnitController unitController;
    private UnitController[] units;

    private Vector3 unitSpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        units = new UnitController[10];
        unitSpawnPosition = new Vector3(0,1,0);
        //unit.transform.SetParent(transform);
        for (int i = 0; i < units.Length - 1; i++)
        {
            //GameObject unit = Instantiate(unitController, unitSpawnPosition, Quaternion.identity);
            unitSpawnPosition.z += 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
