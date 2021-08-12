using System.Collections;
using System.Collections.Generic;
using Items;
using Units;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreUnitList", menuName = "ScriptableObject/StoreUnitList")]
public class StoreUnitList : ScriptableObject
{
    public List<StoreUnit> storeUnits;


    public List<StoreUnit> GetStoreUnits()
    {
        return storeUnits;
    }

    public StoreUnit GetStoreUnitByEntityType(EntityType entityType)
    {
        foreach (var storeUnit in storeUnits)
        {
            if (storeUnit.entityType == entityType)
                return storeUnit;
        }

        return null;
    }
}
