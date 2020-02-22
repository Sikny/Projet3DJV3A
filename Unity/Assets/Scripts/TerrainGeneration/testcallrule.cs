using UnityEngine;

public class testcallrule : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rule r = new Rule("");
        Rule.saveLevel("test1", r);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
