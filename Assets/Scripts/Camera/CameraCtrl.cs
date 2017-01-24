using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

	public enum CAM {
		MIRROR,SIM,MOUSE
	}

	public CAM selected;
	public Transform mirrorCamera;
	public Transform simCamera;
	public Transform mouseCamera;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("ChangeCam")) {
			if(selected == CAM.SIM) {
				simCamera.GetComponent<Camera>().enabled = false;
				mirrorCamera.GetComponent<Camera>().enabled = true;
				mouseCamera.GetComponent<Camera>().enabled = false;
				selected = CAM.MIRROR;
			} else if (selected == CAM.MIRROR) {
				simCamera.GetComponent<Camera>().enabled = false;
				mirrorCamera.GetComponent<Camera>().enabled = false;
				mouseCamera.GetComponent<Camera>().enabled = true;
				selected = CAM.MOUSE;
			} else {
				mirrorCamera.GetComponent<Camera>().enabled = false;
				simCamera.GetComponent<Camera>().enabled = true;
				mouseCamera.GetComponent<Camera>().enabled = false;
				selected = CAM.SIM;
			}
		}
	}
}
