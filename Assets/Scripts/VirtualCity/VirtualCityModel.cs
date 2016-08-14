using UnityEngine;
using System.Collections;

public class VirtualCityModel : MonoBehaviour {

    public GameObject buildingPrefab;
    public int buildingsX = 10;
    public int buildingsY = 10;
    public Color coolColor = Color.blue;
    public Color midColor = Color.yellow;
    public Color hotColor = Color.red;
    public DataDisplay dataDisplay;

    private Building[,] city;
    private SolarRadiation solarRadiation;
    private bool initialized = false;

    public enum DataDisplay {SolarRadiation};

    private float tempTime = -31;

    // Use this for initialization
    void Start () {
        solarRadiation = GetComponent<SolarRadiation>();
        city = new Building[buildingsX, buildingsY];
        for(int i = 0; i < buildingsX; i ++)
        {
            for(int j = buildingsY - 1; j >= 0; j --)
            {
                Building newBuilding = 
                    ((GameObject) Instantiate(
                        buildingPrefab, 
                        this.transform.position + (new Vector3(i, 0, j)), 
                        Quaternion.identity))
                    .GetComponent<Building>();
                newBuilding.transform.parent = this.transform;
                newBuilding.data = this.GetComponent<BuildingDataCtrl>().constructBuildingData(
                    -1, i, j, 0, 0, this.coolColor, this.midColor, this.hotColor);
                city[i, buildingsY - j - 1] = newBuilding;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    internal void updateBuilding(int id, int x, int y, int rotation)
    {
        BuildingData newData = this.GetComponent<BuildingDataCtrl>().constructBuildingData(
                    id, x, y, rotation, 0, this.coolColor, this.midColor, this.hotColor);

        if (this.initialized)
        {
            switch (this.dataDisplay)
            {
                case (DataDisplay.SolarRadiation):
                    this.solarRadiation.updateBuilding(this.city, newData);
                    break;
                default:
                    break;
            }
        } else
        {
            this.city[x, y].updateData(newData);
        }
    }
}