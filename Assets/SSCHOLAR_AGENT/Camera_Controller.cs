using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour {
    public Camera mainCamera;
    public Camera overheadCamera;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
        public void ShowOverheadView()
        {
            mainCamera.enabled = false;
            overheadCamera.enabled = true;
            //set the shading style to show better from a top down view
        }

        public void ShowMainView()
        {
            mainCamera.enabled = true;
            overheadCamera.enabled = false;
        }
    
}
