﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadMapsScene : MonoBehaviour
{
    public GameObject file; //Original

    public List<GameObject> filesList;
    
    public static string currentSel;
    void Start()
    {
        int nb = 0;
        DirectoryInfo dir = new DirectoryInfo("levels/");
        if (!dir.Exists)
        {
            dir.Create();
            Debug.Log("Dossier créé!");
        }
        else
        {
            FileInfo[] info = dir.GetFiles("*.*");
            foreach (FileInfo f in info)
            {
                GameObject fileNew = Instantiate(file, gameObject.transform);

                GameObject textFile = fileNew.transform.Find("textFile").gameObject;
                Text compTextFile = textFile.GetComponent<Text>();
                compTextFile.text = f.Name;
                
                GameObject buttonFile = fileNew.transform.Find("buttonFile").gameObject;
                Button compButtonFile = buttonFile.GetComponent<Button>();
                compButtonFile.onClick.AddListener(delegate { Selectionner(f.Name); });

                RectTransform rectTransform = fileNew.GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(0,-25 - nb++ * 60,0);
                fileNew.name = f.Name;
                fileNew.SetActive(true);
                
                filesList.Add(fileNew);
            }
        }
    }

    void Selectionner(string element)
    {
        currentSel = element;
        Debug.Log(currentSel);
        foreach (var fileTest in filesList)
        {
            Image image = fileTest.GetComponent<Image>();
            if (fileTest.name.Equals(element))
            {
                image.color = new Color(1, 1, 1);
            }
            else
            {
                image.color = new Color(0.8f, 0.8f, 0.8f);
            }
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}