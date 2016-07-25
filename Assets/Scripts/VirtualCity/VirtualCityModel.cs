using UnityEngine;
using System.Collections;

public class VirtualCityModel : MonoBehaviour {

    public int buildingsX;
    public int buildingsY;
    public int buildingWidth;
    public int buildingHeight;
    public DataDisplay dataDisplay;

    private SolarRadiation solarRadiation;

    public enum DataDisplay {SolarRadiation};

	// Use this for initialization
	void Start () {
        solarRadiation = GetComponent<SolarRadiation>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}