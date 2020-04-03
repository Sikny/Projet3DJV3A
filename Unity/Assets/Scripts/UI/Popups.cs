using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Popups : MonoBehaviour
{
    public TextMeshProUGUI popupText;


    #region Singleton
    
    public static Popups instance;
    
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Several instances");
            return;
        }
        instance = this;
    }

    #endregion

    public void Popup(String content, Color color)
    {
        
    }
    
    public void Popup(String content)
    {
        popupText.SetText(content);
        popupText.gameObject.SetActive(true);
        StartCoroutine(DelayCorouting(3));
    }

    IEnumerator DelayCorouting(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        popupText.gameObject.SetActive(false);
    }
}
