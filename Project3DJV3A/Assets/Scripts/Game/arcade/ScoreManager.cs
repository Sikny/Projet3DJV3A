using TMPro;
using UnityEngine;
using Utility;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = "Score : " + GameSingleton.Instance.GetPlayer().currentScore;
    }
}
