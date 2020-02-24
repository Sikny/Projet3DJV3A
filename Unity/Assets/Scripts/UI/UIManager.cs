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
}
