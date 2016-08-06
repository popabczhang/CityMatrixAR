using UnityEngine;
using System.Collections;

public class BuildingData {

    public int id;
    public int x;
    public int y;
    public float height;
    public int rotation;
    public double[,] heatMap;
    public Color coolColor;
    public Color midColor;
    public Color hotColor;

    public BuildingData(int id, int x, int y, int rotation, Color coolColor, Color midColor, Color hotColor)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.rotation = rotation;
        this.coolColor = coolColor;
        this.midColor = midColor;
        this.hotColor = hotColor;

        this.height = 5;
        this.heatMap = new double[7, 7];
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
        int a = 0;
        for(int j = this.heatMap.GetLength(1) - 1; j >= 0; j --)
        {
            for(int i = 0; i < this.heatMap.GetLength(0); i ++)
            {
                double val = this.heatMap[i, j];
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
