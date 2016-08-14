using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BuildingDataCtrl : MonoBehaviour
{
    public TextAsset buildingTypeData;
    public float BuildingBaseWidth = 30;

    private Dictionary<int, BuildingData> types;

    void Awake()
    {
        types = new Dictionary<int, BuildingData>();
        BuildingData tmp = new BuildingData();
        tmp.width = this.BuildingBaseWidth;
        types.Add(-1, tmp);
        string[] entries = buildingTypeData.text.Split('\n');
        string[] categories = entries[0].Split(',');
        foreach(string entry in entries.Skip(1).ToArray())
        {
            if(entry == "")
            {
                break;
            }
            BuildingData type = new BuildingData();
            type.width = this.BuildingBaseWidth;

            int i = 0;
            foreach (string value in entry.Split(','))
            {
                switch(categories[i])
                {
                    case "id":
                        type.id = int.Parse(value);
                        break;
                    case "height":
                        type.height = float.Parse(value);
                        break;
                    default:
                        break;
                }
                i++;
            }
            Debug.Log(type.id.ToString() + " " +  type.height.ToString());
            types[type.id] = type;
        }
    }

    public BuildingData constructBuildingData(int id, int x, int y, int rotation, int magnitude, 
        Color coolColor, Color midColor, Color hotColor)
    {
        BuildingData output;
        if (this.types.ContainsKey(id))
        {
            output = this.types[id].Copy();
        } else
        {
            Debug.LogError("Building type not found: " + id.ToString() + " --Using -1.");
            output = this.types[-1].Copy();
        }
        output.x = x;
        output.y = y;
        output.rotation = rotation;
        output.magnitude = magnitude;
        output.coolColor = coolColor;
        output.midColor = midColor;
        output.hotColor = hotColor;
        return output;
    }
}

public class BuildingData {

    public int id;
    public int x;
    public int y;
    public float height;
    public float width;
    public int rotation;
    public int magnitude;
    public double[,] heatMap;
    public double colorRef = 1;
    public Color coolColor;
    public Color midColor;
    public Color hotColor;

    public BuildingData()
    {
        this.id = -1;
        this.x = 0;
        this.y = 0;
        this.rotation = 0;
        this.coolColor = Color.green;
        this.midColor = Color.red;
        this.hotColor = Color.yellow;

        this.height = 0.1f;
        this.width = 30;
        this.magnitude = 0;
        this.heatMap = new double[7, 7];
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                this.heatMap[i, j] = 0;
            }
        }
    }
    
    public BuildingData Copy()
    {
        return (BuildingData)this.MemberwiseClone();
    }

    internal Color[] getColors()
    {
        Color[] output = new Color[49];
        int a = 0;
        for(int j = this.heatMap.GetLength(1) - 1; j >= 0; j --)
        {
            for(int i = 0; i < this.heatMap.GetLength(0); i ++)
            {
                double val = this.heatMap[i, j] / this.colorRef;
                if (val < 0.5f)
                {
                    output[a] = Color.Lerp(this.coolColor, this.midColor, (float) val * 2f);
                }
                else
                {
                    output[a] = Color.Lerp(this.midColor, this.hotColor, (float) (val - 0.5d) * 2);
                }
                a++;
            }
        }
        
        return output;
    }

}
