using UnityEngine;
using System.Collections;

public class BuildingData {

    public int id;
    public int x;
    public int y;
    public float height;
    public int rotation;
    public float[,] heatMap;
    public Color coolColor;
    public Color hotColor;

    public BuildingData(int id, int x, int y, int rotation, Color coolColor, Color hotColor)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.rotation = rotation;
        this.coolColor = coolColor;
        this.hotColor = hotColor;

        this.height = 5;
        this.heatMap = new float[7, 7];
        for(int i = 0; i < 7; i ++)
        {
            for(int j = 0; j < 7; j ++)
            {
                this.heatMap[i, j] = 0;
            }
        }
    }

    internal Color[] getColors()
    {
        Color[] output = new Color[49];
        int i = 0;
        foreach (float val in this.heatMap)
        {
            output[i] = Color.Lerp(this.coolColor, this.hotColor, val);
            i++;
        }
        
        return output;
    }

}
