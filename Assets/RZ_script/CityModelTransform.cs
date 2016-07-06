using UnityEngine;
using System.Collections;

public class CityModelTransform : MonoBehaviour {
	
	public GameObject CityModel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		CityModel.transform.position.Set ((float)CityModel.transform.position.x - 0.1f, (float)CityModel.transform.position.y, (float)CityModel.transform.position.z);
	}

	/*
	public void xMinusDotOne () {
		//float x = CityModel.transform.position.x;
		CityModel.transform.position.Set ((float)CityModel.transform.position.x - 0.1f, (float)CityModel.transform.position.y, (float)CityModel.transform.position.z);
	}

	public void xMinusOne () {
		//float x = CityModel.transform.position.x;
		CityModel.transform.position.Set ((float)CityModel.transform.position.x - 1.0f, (float)CityModel.transform.position.y, (float)CityModel.transform.position.z);
	}

	public void xMinusTen () {
		//float x = CityModel.transform.position.x;
		CityModel.transform.position.Set ((float)CityModel.transform.position.x - 10.0f, (float)CityModel.transform.position.y, (float)CityModel.transform.position.z);
	}

	public void xMinusHundred () {
		//float x = CityModel.transform.position.x;
		CityModel.transform.position.Set ((float)CityModel.transform.position.x - 100.0f, (float)CityModel.transform.position.y, (float)CityModel.transform.position.z);
	}
	*/
}
