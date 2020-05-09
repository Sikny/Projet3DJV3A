﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorMap : MonoBehaviour
{
    [SerializeField] public ToolsEnum currentTool = ToolsEnum.MAP_BRUSH;
    
    public GameObject UIToolSelect;
    public GameObject UIToolRadius;
    public GameObject UIToolAmplitude;
    public GameObject UIMinHeight;
    public GameObject UIMaxHeight;
    
    public Camera camera;
    public GameObject mapObject;

    public Material[] materialMatchingToToolMode;
    public enum ToolsEnum
    {
        MAP_BRUSH,
        DIFFICULTY_PAINTING
    }

    private Map map;
    private int radiusTool = 20;
    private int amplitudeTool = 10;
    private float minHeight = -10;
    private float maxHeight = 10;
    
    public void setRadiusTool()
    {
        Slider slider = UIToolRadius.GetComponent<Slider>();
        radiusTool = (int)slider.value;
    }
    
    public void setAmplitudeTool()
    {
        Slider slider = UIToolAmplitude.GetComponent<Slider>();
        amplitudeTool = (int)slider.value;
    }
    
    public void setMinHeight()
    {
        TMP_InputField inputField = UIMinHeight.GetComponent<TMP_InputField>();
        minHeight = float.Parse(inputField.text);
    }
    public void setMaxHeight()
    {
        TMP_InputField inputField = UIMaxHeight.GetComponent<TMP_InputField>();
        maxHeight = float.Parse(inputField.text);
    }
    
    public void setTool()
    {
        Dropdown dropdown = UIToolSelect.GetComponent<Dropdown>();
        switch (dropdown.value)
        {
            case 0:
                currentTool = ToolsEnum.MAP_BRUSH;
                mapObject.GetComponent<MeshRenderer>().material = materialMatchingToToolMode[0];
                break;
            case 1:
                currentTool = ToolsEnum.DIFFICULTY_PAINTING;
                mapObject.GetComponent<MeshRenderer>().material = materialMatchingToToolMode[1];
                break;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        map = new Map(camera,mapObject);
    }

    // Update is called once per frame
    void Update()
    {
        map.updateMapProperties( minHeight, maxHeight);
        map.UpdateMap(currentTool,radiusTool, amplitudeTool);
    }
}