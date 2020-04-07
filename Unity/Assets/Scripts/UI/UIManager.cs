using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI currencyText;
    private int currency;
    // Start is called before the first frame update
    void Start()
    {
        currency = 10;
        currencyText.SetText(currency + "g");
    }
    
    //utils
    public static void clearUI(GameObject parent, int ignoreIndex)
    {
        for (int i = 1; i < parent.transform.childCount; i++)
        {
            if(ignoreIndex != i)
                parent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
