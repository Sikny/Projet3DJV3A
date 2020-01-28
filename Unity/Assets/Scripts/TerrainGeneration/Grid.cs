using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int[,] gridArray;
    private String printGrid = "\n";

    public string PrintGrid
    {
        get => printGrid;
        set => printGrid = value;
    }

    public int[,] GridArray
    {
        get => gridArray;
        set => gridArray = value;
    }

    private void Start()    
    {
        
    }
}
