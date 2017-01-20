using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

	public enum CAM {
		MIRROR,SIM
	}

	public CAM selected;
	public Transform mirrorCamera;
	public Transform simCamera;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("ChangeCam")) {
			if(selected == CAM.SIM) {
				simCamera.GetComponent<Camera>().enabled = false;
				mirrorCamera.GetComponent<Camera>().enabled = true;
				selected = CAM.MIRROR;
			} else {
				mirrorCamera.GetComponent<Camera>().enabled = false;
				simCamera.GetComponent<Camera>().enabled = true;
				selected = CAM.SIM;
			}
		}
	}
}
