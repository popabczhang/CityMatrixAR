using UnityEngine;
using System.Collections;

public class VirtualCityModel : MonoBehaviour {

    public GameObject buildingPrefab;
    public int buildingsX = 10;
    public int buildingsY = 10;
    public Color coolColor = Color.blue;
    public Color hotColor = Color.red;
    public DataDisplay dataDisplay;

    private Building[,] city;
    private SolarRadiation solarRadiation;

    public enum DataDisplay {SolarRadiation};

	// Use this for initialization
	void Awake () {
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
                newBuilding.data = new BuildingData(
                    -1, i, j, 0, this.coolColor, this.hotColor);
                city[i, j] = newBuilding;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

	}

    internal void editBuilding(int id, int x, int y, int rotation)
    {
        this.city[x, y].updateData(
            new BuildingData(id, x, y, rotation, this.coolColor, this.hotColor));
    }

    /// <summary>
    /// Alters the height of the specified building.
    /// </summary>
    /// <param name="x">The x coordinate of the building.</param>
    /// <param name="y">The y coordinate of the building.</param>
    /// <param name="newHeight">The new height of the building.</param>
    void changeBuildingHeight(int x, int y, float newHeight)
    {
        this.city[x, y].changeHeight(newHeight);
    }

    /// <summary>
    /// Recolors the specified building using the given flattened array of colors.
    /// 
    /// Flattened array order: (0, 0), (1, 0) ... (0, 1), (1, 1) ...
    /// </summary>
    /// <param name="x">The x coordinate of the building.</param>
    /// <param name="y">The y coordinate of the building.</param>
    /// <param name="colors">The flattened array of new colors to replace the old ones with.</param>
    void changeBuildingColors(int x, int y, Color[] colors)
    {
        if (colors.Length < 7)
        {
            throw new System.Exception("Malformed colors array.");
        }
        this.city[x, y].recolor(colors);
    }

    /// <summary>
    /// Changes the height of each building in the city.
    /// </summary>
    /// <param name="newHeights">2D array of new heights mapping to each building in the city.</param>
    void changeAllBuildingHeights(float[,] newHeights)
    {
        if(newHeights.GetLength(0) != city.GetLength(0) || 
            newHeights.GetLength(1) != city.GetLength(1))
        {
            throw new System.Exception("Wrong sized array input!");
        }

        for(int i = 0; i < newHeights.GetLength(0); i ++)
        {
            for(int j = 0; j < newHeights.GetLength(1); j ++)
            {
                this.changeBuildingHeight(i, j, newHeights[i, j]);
            }
        }
    }

    /// <summary>
    /// Changes the colors of each building in the city.
    /// </summary>
    /// <param name="colors">2D array of new colors mapping to each building in the city.</param>
    void changeAllBuildingColors(Color[,][] colors)
    {
        if (colors.GetLength(0) != city.GetLength(0) ||
            colors.GetLength(1) != city.GetLength(1))
        {
            throw new System.Exception("Wrong sized array input!");
        }

        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                this.changeBuildingColors(i, j, colors[i, j]);
            }
        }
    }
}