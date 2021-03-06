﻿using System;
using System.IO;
using Language;
using LevelEditor;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor {
    public class EditorMap : MonoBehaviour
    {
        [SerializeField] public ToolsEnum currentTool = ToolsEnum.MAP_BRUSH;
    
        public GameObject UIToolSelect;
        public GameObject UIToolRadius;
        public GameObject UIToolAmplitude;
        public GameObject UIMinHeight;
        public GameObject UIMaxHeight;

        public GameObject canvasTool;
        public GameObject canvasInit;
    
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
    
        public string Size { get; set; }
    
        public string Filename { get; set; }
    
        public string Money { get; set; }

        
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

        public void generate()
        {
            int sizeDemande = int.Parse(Size);
            int moneyDemande = int.Parse(Money);
            if (10 > sizeDemande || sizeDemande > 100)
            {
                throw new Exception("size invalide" );
            }

            if (0 > moneyDemande || moneyDemande > 10000)
            {
                throw new Exception("money invalide" );
            }

            if (string.IsNullOrWhiteSpace(Filename))
            {
                Popups.instance.Popup("Level name required.", Color.red);
                throw new Exception("filename invalide" );
            }

            if (File.Exists(Application.persistentDataPath + "/" + Filename + ".lvl"))
            {
                map = Map.Load(camera, mapObject, Filename);
            }
            else
            {
                map = new Map(camera, mapObject,sizeDemande ,moneyDemande , Filename);
            }
            
            canvasTool.SetActive(true);
            canvasInit.SetActive(false);
       
        }

        public void save()
        {
            if (map != null)
            {
                map.Save();
                Popups.instance.Popup(Traducer.Translate("Map saved!"));
            }
            else
            {
                Popups.instance.Popup(Traducer.Translate("Select settings first!"), Color.red);
            }

        }

        private void Start()
        {
            Size = "50";
            Money = "200";
            Dropdown dropdown = UIToolSelect.GetComponent<Dropdown>();
            dropdown.options[0].text = Traducer.Translate("Brush");
            dropdown.options[1].text = Traducer.Translate("Difficulty Brush");
        }

        // Update is called once per frame
        void Update()
        {
            if (map != null)
            {
                map.UpdateMapProperties(minHeight, maxHeight);
                map.UpdateMap(currentTool, radiusTool, amplitudeTool);
            }
        }
    }
}
