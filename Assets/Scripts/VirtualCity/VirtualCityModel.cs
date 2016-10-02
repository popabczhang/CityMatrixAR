using UnityEngine;
using System.Collections;

public class VirtualCityModel : MonoBehaviour {

    public int buildingsX = 10;
    public int buildingsY = 10;
    public string grasshopperFileDirectory;

    private BuildingModel[,] city;
    private SolarRadiation solarRadiation;
    private bool initialized = false;

    // Use this for initialization
    void Start()
    {
        solarRadiation = GetComponent<SolarRadiation>();
        city = new BuildingModel[buildingsX, buildingsY];
        for (int i = 0; i < buildingsX; i++)
        {
            for (int j = 0; j < buildingsY; j++)
            {
                city[i, j] = new BuildingModel();
                city[i, j].parentModel = this;
            }
        }
    }

    float tempt = 0;
	// Update is called once per frame
	void Update () {
        tempt += Time.deltaTime;
        if(Input.GetButtonDown("RunSimulation") && tempt > 3)
        {
            StartCoroutine("RunSolarSimulation");
            tempt = 0;
        }
    }

    internal BuildingModel[,] GetCity()
    {
        return (BuildingModel[,]) this.city;
    }

    internal void InitializeView(int i, int j, Building b)
    {
        if(i >= 0 && i < this.buildingsX && j >= 0 && j < this.buildingsY)
        {
            this.city[i, j].AddView(b);
        }
    }

    IEnumerator RunSolarSimulation()
    {
        Debug.Log("Starting Solar Simulation");
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