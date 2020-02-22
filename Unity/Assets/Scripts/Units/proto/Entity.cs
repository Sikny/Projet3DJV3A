using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public readonly GameObject associedGameObject;
    private int strength; // j'ai pas lu les specs mais ca doit etre une bonne idée
    private int life;

    public Entity(GameObject associedGameObject)
    {
        this.associedGameObject = associedGameObject;
        this.strength = 5;
        this.life = 100; // ?
    }

    public void kill()
    {
        GameObject.Destroy(associedGameObject);
    }
    
    public int changeLife(int deltaValue)
    {
        life += deltaValue;
        if (life > 100) life = 100;
        else if (life < 0) life = 0;

        if (life == 0) kill();
        
        return life;

    }

    public int getStrength()
    {
        return strength;
    }
}
