using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnecteMode : MonoBehaviour
{
    public TMP_Text connecteIndication;
    // Start is called before the first frame update
    void Start()
    {
         updateConnecteIndication();
    }
    public void updateConnecteIndication()
    {
        if (connecteIndication != null)
        {
            string token = PlayerPrefs.GetString("connection.token");
            if (token == null || token.Length < 8)
            {
                connecteIndication.text = "Non-connecté";
                connecteIndication.color = Color.white;
            }
            else
            {
                connecteIndication.text = "Connecté";
                connecteIndication.color = Color.green;
            }
        }
    }

    public void Update()
    {
        updateConnecteIndication();
    }
}
