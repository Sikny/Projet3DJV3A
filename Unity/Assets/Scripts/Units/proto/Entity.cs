using System;
using Units.proto;
using UnitSystem;
using UnityEngine;

public class Entity : MonoBehaviour {
    private int _strength; // j'ai pas lu les specs mais ca doit etre une bonne idée
    private int _life;
    public AbstractUnit parentUnit;
    public SystemUnit system;
    public bool selectable;

    public MeshRenderer meshRenderer;

    private Color _firstColor;

    private void Awake() {
        _strength = 5;
        _life = 100;
        _firstColor = meshRenderer.material.color;

        system = FindObjectOfType<SystemUnit>();
    }

    public void InitColor(Color col) {
        meshRenderer.material.color = _firstColor;
        _firstColor = col;
    }

    public void ResetColor() {
        meshRenderer.material.color = _firstColor;
    }

    public void SetParent(AbstractUnit unit) {
        parentUnit = unit;
    }

    private void KillEntity() {
        Destroy(gameObject);
    }

    private void OnMouseEnter() {
        if(selectable)
            system.SelectUnit((RemotedUnit)parentUnit);
    }

    private void OnMouseExit() {
        system.SelectUnit(null);
    }

    public int ChangeLife(int deltaValue) {
        _life += deltaValue;
        if (_life > 100) _life = 100;
        else if (_life < 0) _life = 0;
        if (_life == 0) KillEntity();
        return _life;
    }

    public int GetStrength() {
        return _strength;
    }
}
