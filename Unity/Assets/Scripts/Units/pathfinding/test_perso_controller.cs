using System.Collections;
using System.Collections.Generic;
using AStar;
using UnityEngine;

public class test_perso_controller : MonoBehaviour
{
    public GameObject worldProvider;
    private Stack<Tile> itineraire;

    private test_worldgen gen;
    // Start is called before the first frame update
    void Start()
    {
        gen = worldProvider.GetComponent<test_worldgen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (gen.alg != null && gen.graph != null)
                {
                    Vector3 exitPos = hit.transform.position;
                    gen.graph.BeginningNode = gen.tiles[(int)transform.position.x, (int)transform.position.z];
                    gen.graph.ExitNode = gen.tiles[(int)exitPos.x, (int)exitPos.z];
                    gen.alg.Solve();
                    itineraire = gen.graph.ReconstructPath();
                }
            }
        }
        
        Vector3 last = transform.position;
        if (itineraire != null && itineraire.Count > 0)
        {
            Vector3 posTarget = itineraire.Peek().Pos;
            if (Vector3.Distance(last, posTarget) < 0.25f)
            {
                itineraire.Pop();
            } 
            transform.position = Vector3.MoveTowards(last, posTarget, 5f * Time.deltaTime);
        }
    }
}
