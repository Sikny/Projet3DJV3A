using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SeriaVector3
{
    private float x, y, z;

    public SeriaVector3(float x,float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float X
    {
        get => x;
        set => x = value;
    }
    public float Y
    {
        get => y;
        set => y = value;
    }
    public float Z
    {
        get => z;
        set => z = value;
    }

    public override string ToString()
    {
        return "vec3(" + x + ","+y+"," + z + ")";
    }

}
