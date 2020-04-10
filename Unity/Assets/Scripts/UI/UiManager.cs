using UnityEngine;

namespace UI {
    public class UiManager : MonoBehaviour {
        public GameObject shopPanel;
        public GameObject inventoryPanel;
        
        public static void ClearUi(GameObject parent, int ignoreIndex)
        {
            for (int i = 1; i < parent.transform.childCount; i++)
            {
                if(ignoreIndex != i)
                    parent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public void ToggleInventory() {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            shopPanel.SetActive(false);
        }

        public void ToggleShop() {
            shopPanel.SetActive(!shopPanel.activeSelf);
            inventoryPanel.SetActive(false);
        }
    }
}
