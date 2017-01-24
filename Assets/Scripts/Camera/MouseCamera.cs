using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour {

	public Transform focus;
	public float scrollMultiplier;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis("Mouse X");
		float y = Input.GetAxis("Mouse Y");
		float z = Input.GetAxis("Mouse ScrollWheel") * scrollMultiplier;
		Vector3 delta = new Vector3(x, y, z);
		this.transform.Translate(delta, Space.Self);
	}

	void OnPreRender() {
		this.transform.LookAt(focus);
	}
}
