using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHighlighter : MonoBehaviour
{       
    private Color startColor = Color.white;
    void OnMouseEnter()
    {
        startColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.yellow;
    }
    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = startColor;
    }
}
