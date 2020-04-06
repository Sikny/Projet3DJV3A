﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units {
    public enum EntityType {
        Soldier, Archer, Mage
    }
    [Serializable]
    public class EntityStruct {
        public EntityType idType;
        public Entity entityPrefab;
        public string description;
    }
    
    [CreateAssetMenu(fileName = "EntityDict", menuName = "ScriptableObjects/EntityDict")]
    public class EntityDict : ScriptableObject {
        public List<EntityStruct> entitiesList;

        public Entity GetEntityType(EntityType idType) {
            int listLen = entitiesList.Count;
            for (int i = 0; i < listLen; i++) {
                if (idType == entitiesList[i].idType)
                    return entitiesList[i].entityPrefab;
            }
            return null;
        }
    }
}
