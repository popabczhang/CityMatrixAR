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
    void Awake () {
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
                newBuilding.data = new BuildingData(
                    -1, i, j, 0, this.coolColor, this.midColor, this.hotColor);
                city[i, buildingsY - j - 1] = newBuilding;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (tempTime < -30)
        {
            foreach (Building b in this.city)
            {
                b.changeHeight(Random.Range(1, 10));
            }
            SolarRadiationSimulation sim = this.gameObject.AddComponent<SolarRadiationSimulation>();
            sim.initialize(this.city);
            tempTime = -20;
        }
        if(tempTime > 5)
        {
            int x = Random.Range(0, buildingsX);
            int y = Random.Range(0, buildingsY);
            int h = Random.Range(1, 5);
            Debug.Log(x + " " + y + " " + h);
            tempTime = 0;

            this.solarRadiation.
                changeBuildingHeight(this.city,x,y,h);
        }
        tempTime += Time.deltaTime;
    }

    internal void updateBuilding(int id, int x, int y, int rotation)
    {
        BuildingData newData = new BuildingData(id, x, y, rotation, this.coolColor, this.midColor, this.hotColor);
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