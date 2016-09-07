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
    public string grasshopperFileDirectory;

    private Building[,] city;
    private SolarRadiation solarRadiation;
    private bool initialized = false;

    public enum DataDisplay {SolarRadiation};

    public bool useWireframe = false;

    private float tempTime = -31;

    // Use this for initialization
    void Start()
    {
        solarRadiation = GetComponent<SolarRadiation>();
        city = new Building[buildingsX, buildingsY];
        for (int i = 0; i < buildingsX; i++)
        {
            for (int j = buildingsY - 1; j >= 0; j--)
            {
                Building newBuilding =
                    ((GameObject)Instantiate(
                        buildingPrefab,
                        this.transform.position + new Vector3(i * this.transform.localScale.x, 0, j * this.transform.localScale.z),
                        Quaternion.identity))
                    .GetComponent<Building>();
                newBuilding.drawWireframe = this.useWireframe;
                newBuilding.transform.parent = this.transform;
                newBuilding.data = this.GetComponent<BuildingDataCtrl>().constructBuildingData(
                    -1, i, j, 0, 0, this.coolColor, this.midColor, this.hotColor);
                city[i, buildingsY - j - 1] = newBuilding;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("RunSimulation"))
        {
            StartCoroutine("RunSolarSimulation");
        }
    }

    internal Building[,] GetCity()
    {
        return this.city;
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
                    this.city[x, y].updateData(newData);
                    break;
            }
        } else
        {
            this.city[x, y].updateData(newData);
        }
    }

    internal void changeBuildingHeight(int x, int y, float newHeight)
    {
        BuildingData newData = this.city[x, y].data.Copy();
        newData.height = newHeight;

        if (this.initialized)
        {
            switch (this.dataDisplay)
            {
                case (DataDisplay.SolarRadiation):
                    this.solarRadiation.updateBuilding(this.city, newData);
                    break;
                default:
                    this.city[x, y].updateData(newData);
                    break;
            }
        }
        else
        {
            this.city[x, y].updateData(newData);
        }
    }

    IEnumerator RunSolarSimulation()
    {
        float time = 0;
        while(time < 1)
        {
            time += Time.deltaTime;
            yield return null;
        }

        SolarRadiationSimulation tmp = this.gameObject.AddComponent<SolarRadiationSimulation>();
        tmp.workingDirectory = this.grasshopperFileDirectory;
        tmp.initialize(this.city);
        this.initialized = true;
    }
}