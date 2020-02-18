using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public readonly GameObject associedGameObject;
    public int strength; // j'ai pas lu les specs mais ca doit etre une bonne idée
    public int life;

    public Entity(GameObject associedGameObject)
    {
        this.associedGameObject = associedGameObject;
        this.strength = 50;
        this.life = 100; // ?
    }
}
