using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebClient;

public class SummaryScores : MonoBehaviour
{
    public GameObject score; //Original

    public List<GameObject> scoresList;

    public bool called = true;
    public int offset = 0;
    
    // Start is called before the first frame update
    void Update()
    {
        if (!called)
        {
            StartCoroutine(ConnectModule.Instance.BestScores(ProcessConnectionResult));
            called = true;
        }
        
    }

    public class ScoreJson
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string nbpoints { get; set; }
        public string begindate { get; set; }
        public string timegame { get; set; }
        public string seed { get; set; }
    }
    
    private void ProcessConnectionResult(UnityWebRequest www) {
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            //Debug.Log("Connect success");
            string result = www.downloadHandler.text;
            //Debug.Log("Received: " + result);
            
            int nb = 0;
            var json = JsonConvert.DeserializeObject<List<ScoreJson>>(result);
            foreach (var scoreJson in json)
            {
                GameObject scoreNew = Instantiate(score, gameObject.transform);

                GameObject nameScoreGO = scoreNew.transform.Find("nameScore").gameObject;
                Text compNameFile = nameScoreGO.GetComponent<Text>();
                compNameFile.text = scoreJson.firstname + " " + scoreJson.lastname;
                
                GameObject pointsScoreGO = scoreNew.transform.Find("pointScore").gameObject;
                Text compPointsFile = pointsScoreGO.GetComponent<Text>();
                compPointsFile.text = "Score : " + scoreJson.nbpoints;
                
                GameObject classScoreGO = scoreNew.transform.Find("classScore").gameObject;
                Text compClassFile = classScoreGO.GetComponent<Text>();
                compClassFile.text = "#" + (offset+nb+1);

                RectTransform rectTransform = scoreNew.GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(400,-25 - nb++ * 60,0);
                scoreNew.name = scoreJson.firstname + " " + scoreJson.lastname;
                scoreNew.SetActive(true);
                
                Image image = scoreNew.GetComponent<Image>();
                switch (nb)
                {
                    case 1: image.color =  new Color(0xef/255f, 0xd8/255f, 0x07/255f);
                        break;
                    case 2 : image.color = Color.gray;
                        break;
                    case 3 : image.color = new Color(0x61/255f,0x4e/255f,0x1a/255f);
                        break;
                    default:  image.color = Color.white;
                        break;
                }
                
                scoresList.Add(scoreNew);
            }
            
        }
    }
}
