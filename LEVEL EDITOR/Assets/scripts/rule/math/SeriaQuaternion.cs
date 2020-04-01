using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SeriaQuaternion
{
    private float x, y, z, w;

    public SeriaQuaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
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
    public float W
    {
        get => w;
        set => w = value;
    }

    public override string ToString()
    {
        return "vec4(" + x + "," + y + "," + z + ","+w+")";
    }

}
