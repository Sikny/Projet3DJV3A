using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Networking;
using Utility;
using WebClient;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class EndGame : MonoBehaviour
{
    public TMP_Text scoreText;

    public TMP_Text classementText;

    public bool called = false;

    public SummaryScores summaryScores;

    private const int SCORE_MIDDLE = 10;

    public GameObject endGamePanel;
    
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            scoreText.text = "Score : " + GameSingleton.Instance.GetPlayer().currentScore;
            if (!called)
            {
                callWS();
                called = true;
            }
        }
    }
    public void callWS()
    {
        string token = PlayerPrefs.GetString("connection.token");
        int nbpoints = GameSingleton.Instance.GetPlayer().currentScore;
        StartCoroutine(ConnectModule.Instance.RegisterScore(token,0, nbpoints, 1024,
            ProcessScoreResult));
    }

    private void ProcessScoreResult(UnityWebRequest www) {
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            string result = www.downloadHandler.text;
            if (result != null && !result.Contains("NOK"))
            {
                classementText.text = "Classement : #" + result;
                summaryScores.called = false;
            }
            else
            {
                classementText.text = "Impossible d'établir le classement";
            }
        }
            
    }

    public void Back()
    {
        endGamePanel.SetActive(false);
        SceneManager.LoadScene("Menu");
        endGamePanel.SetActive(false);

    }
}
