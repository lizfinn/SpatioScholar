using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SScholar_Agent_Target_Logic : MonoBehaviour {

    public Vector3 GetTargetLocation()
    {
        Vector3 target_coordinates = new Vector3(0,0,0);
        //return a random location within the bounding box of this object

        //get mesh filter bounds
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;
        float tempx = Random.Range(0, bounds.size.x);
        float tempy = Random.Range(0, bounds.size.y);
        float tempz = Random.Range(0, bounds.size.z);

        target_coordinates = gameObject.transform.position;
        target_coordinates.x = target_coordinates.x - (bounds.size.x / 2) + tempx;
        target_coordinates.y = target_coordinates.y - (bounds.size.y / 2) + tempy;
        target_coordinates.z = target_coordinates.z - (bounds.size.z / 2) + tempz;

        return target_coordinates;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
