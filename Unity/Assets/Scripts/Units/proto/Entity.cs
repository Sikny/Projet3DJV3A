using UnityEngine;

public class Entity {
    public readonly GameObject associedGameObject;
    private int strength; // j'ai pas lu les specs mais ca doit etre une bonne idée
    private int life;

    public Entity(GameObject associedGameObject) {
        this.associedGameObject = associedGameObject;
        strength = 5;
        life = 100; // ?
    }

    public void KillEntity() {
        Object.Destroy(associedGameObject);
    }
    
    public int ChangeLife(int deltaValue) {
        life += deltaValue;
        if (life > 100) life = 100;
        else if (life < 0) life = 0;
        if (life == 0) KillEntity();
        return life;
    }

    public int GetStrength() {
        return strength;
    }
}
