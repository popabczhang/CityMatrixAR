using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Linq;
using System.Diagnostics;


public class SolarRadiationSimulation : MonoBehaviour{


    private Building[,] city;
    private string heightDataPath;
    private string sensorDataPath;
    public string workingDirectory;
    private string scriptName;
    
    internal void initialize(Building[,] city)
    {
        this.city = city;
        this.heightDataPath = 
            Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments) + "/CityMatrix/city-heights.txt";
        this.sensorDataPath =
            Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments) + "/CityMatrix/sensor-data.txt";
        if(File.Exists(this.sensorDataPath))
        {
            File.Delete(this.sensorDataPath);
        }
        this.scriptName = this.workingDirectory + "/run.bat";

        this.WriteHeightFile();
        ProcessStartInfo ghProc = new ProcessStartInfo();
        ghProc.WorkingDirectory = this.workingDirectory;
        ghProc.FileName = this.scriptName;

        Process.Start(ghProc);
        StartCoroutine("ReadOutput");
    }

    private void WriteHeightFile()
    {
        string[] output = new string[city.GetLength(1)];

        for (int j = 0; j < city.GetLength(1); j++)
        {
            output[j] = city[0, j].data.width.ToString() + "-" + city[0, j].data.height.ToString();
            for (int i = 1; i < city.GetLength(0); i ++)
            {
                output[j] = output[j] + "," + city[i, j].data.width.ToString() + "-" + city[i, j].data.height.ToString();
            }
        }
        System.IO.File.WriteAllLines(this.heightDataPath, output);
    }
    
    IEnumerator ReadOutput()
    {
        while (!File.Exists(this.sensorDataPath))
        {
            yield return null;
        }

        double[] data = new Double[city.Length * 49];
        StreamReader dataStream = new StreamReader(this.sensorDataPath);
        int i = 0;
        while(!dataStream.EndOfStream)
        {
            data[i] = Double.Parse(dataStream.ReadLine());
            i++;
        }
        dataStream.Close();
        this.ApplyData(data);
    } 

    private void ApplyData(double[] data)
    {
        double max = data.Max();

        int a = 0;
        foreach(Building b in this.city)
        {
            for(int i = 0; i < 7; i ++)
            {
                for(int j = 0; j < 7; j ++)
                {
                    b.data.heatMap[i, j] = data[a];
                    b.data.colorRef = max;
                    a++;
                }
            }
            b.updateData(b.data);
        }
    }
}
