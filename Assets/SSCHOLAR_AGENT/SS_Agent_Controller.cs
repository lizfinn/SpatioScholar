using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SS_Agent_Controller : MonoBehaviour {
    public NavMeshAgent agent1; //need to make these into a dictionary
    public Camera cam;
    public NavMeshAgent agent2;


    // Use this for initialization
    void Start () {
        print("SS_Agent_Controller initialized");
	}

    // Update is called once per frame
    void Update()
    {
        /* OLD CODE FROM MANUAL TARGET SETTING
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Move the Agents, need to update this to reflect the agents.
                agent1.SetDestination(hit.point);
                agent2.SetDestination(hit.point);
            }

        }
        */
    }
}

