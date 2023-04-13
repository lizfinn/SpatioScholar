using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAgent_Intervisibility_Target : MonoBehaviour {

    //List of GameObjects used for intervisibility testing. Realistically, this will often be more than one object
    List<GameObject> IntervisibilityTargets = new List<GameObject>();

    // Use this for initialization
    void Start () {
        Debug.Log("SSAgent_Intervisibility_Target script Start() called");
        //assign this object to some central list of Intervisibility Targets on a master controller
        //this list will be queried by agents to direct their rays only in the direction of intervisbility targets
        //agents will then hold values for how much of the target they can see....not sure the best way to determine how much they can see......

        GameObject controller = GameObject.Find("SScholar_Agent_Controller");
        SScholar_Agent_Controller example = (SScholar_Agent_Controller)controller.GetComponent(typeof(SScholar_Agent_Controller));
        example.AddIntervisibilityTarget(gameObject);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
