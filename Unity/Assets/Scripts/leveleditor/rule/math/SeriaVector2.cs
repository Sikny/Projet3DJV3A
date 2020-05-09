using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SeriaVector2
{
    private float x, z;

    public SeriaVector2(float x, float z)
    {
        this.x = x;
        this.z = z;
    }

    public float X {
        get => x;
        set => x = value;
    }
    public float Z {
        get => z;
        set => z = value;
    }

    public override string ToString()
    {
        return "vec2(" + x + "," + z + ")";
    }

}
