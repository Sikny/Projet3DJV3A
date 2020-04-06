using UnityEngine;

public class ShortcutManager : MonoBehaviour
{

    public GameObject shopPanel;
    public GameObject inventoryPanel;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
            inventoryPanel.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
        
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            shopPanel.SetActive(false);

        }
    }
}
