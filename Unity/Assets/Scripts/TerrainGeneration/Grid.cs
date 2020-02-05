using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private static Grid instance;

    private int[,] gridArray;
    private String printGrid;

    private int tileX;
    private int tileZ;

    private int width;
    private int height;


    private Renderer[,] cubeRenderers;

    private Grid()
    {
        gridArray = new int[500,500];
        printGrid = "\n";

        tileX = 5;
        tileZ = 5;
        
        cubeRenderers = new Renderer[500,500];
    }

        

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

    public int Width
    {
        get => width;
        set => width = value;
    }

    public int Height
    {
        get => height;
        set => height = value;
    }

    public int TileX
    {
        get => tileX;
        set => tileX = value;
    }

    public int TileZ
    {
        get => tileZ;
        set => tileZ = value;
    }

    public Renderer[,] CubeRenderers
    {
        get => cubeRenderers;
        set => cubeRenderers = value;
    }

    public static Grid getInstance()
    {
        if(instance == null)
            instance = new Grid();
        
        return instance;
    }
    
    
}
