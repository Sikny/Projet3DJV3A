using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleIOFile : MonoBehaviour
{
    static private Rule ruleLevel = null;
    void Start()
    {
        ruleLevel = new Rule();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(ruleLevel != null)
            {
                Rule.saveLevel("test",ruleLevel);
            }
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {
            if(ruleLevel == null)
            {
                ruleLevel = Rule.readLevel("test");
            }
        }
    }
}
