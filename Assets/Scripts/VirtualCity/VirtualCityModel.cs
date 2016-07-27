using UnityEngine;
using System.Collections;

public class VirtualCityModel : MonoBehaviour {

    public GameObject buildingPrefab;
    public int buildingsX = 10;
    public int buildingsY = 10;
    public int buildingWidth = 7;
    public int buildingHeight = 7;
    public DataDisplay dataDisplay;

    private Building[,] city;
    private SolarRadiation solarRadiation;

    public enum DataDisplay {SolarRadiation};

	// Use this for initialization
	void Start () {
        solarRadiation = GetComponent<SolarRadiation>();
        city = new Building[buildingsX, buildingsY];
        for(int i = 0; i < buildingsX; i ++)
        {
            for(int j = 0; j < buildingsY; j ++)
            {
                Building newBuilding = 
                    ((GameObject) Instantiate(
                        buildingPrefab, 
                        new Vector3(i, 0, j), 
                        Quaternion.identity))
                    .GetComponent<Building>();
                newBuilding.transform.parent = this.transform;
                newBuilding.changeHeight(0);
                city[i, j] = newBuilding;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}