using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.Rendering;

namespace Utility {
    [Serializable]
    public struct EntitySprite {
        public EntityType entityType;
        public Sprite sprite;
    }
    public class EntityTypeToSprite : MonoBehaviour
    {
        public EntitySprite[] entitySprites;

        public Dictionary<EntityType, Sprite> dict;

        public Sprite GetEntitySprite(EntityType entityType)
        {
            int i;
            for (i = 0; i < entitySprites.Length; i++)
            {
                if (entityType == entitySprites[i].entityType)
                {
                    break;
                }
            }
            return entitySprites[i].sprite;
        }
    }
}