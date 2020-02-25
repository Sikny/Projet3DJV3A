﻿using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    [NonSerialized]
    public bool isFocus = false;
    private bool hasInteracted = false;

    public virtual void Interact()
    {
        #if UNITY_EDITOR
            Debug.Log("interacting with " + transform.name);
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if (isFocus)
        {
            Interact();
            hasInteracted = true;
        }
    }
    
    // Called when the object starts being focused
    public void OnFocused ()
    {
        isFocus = true;
        hasInteracted = false;
    }

    // Called when the object is no longer focused
    public void OnDefocused ()
    {
        isFocus = false;
        hasInteracted = false;
    }
}
