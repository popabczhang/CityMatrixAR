﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class ColorizerCtrl : MonoBehaviour {

    public VirtualCityModel cityModel;
    public string JsonURL;

    private JSONCityMatrix oldData;
    private BuildingModel[,] city;

    //private StreamReader sReader;
    //private string readJson;

    private static string udpString;  //  A new Static variable to hold our score.


    // Use this for initialization
    void Start () {
        //StartCoroutine("Initialize");
        city = cityModel.GetCity();
        //sReader = new StreamReader("C:\\Users\\RYAN\\Dropbox (MIT)\01_Work\\MAS\\06_Fall 2016\\CityMatrix\\01_Software\\Processing\\Colortizer\\all.json");
        //sReader = new StreamReader("C:/Users/RYAN/Dropbox (MIT)/01_Work/MAS/06_Fall 2016/CityMatrix/01_Software/Processing/Colortizer/all.json");

        //udpString = UDPReceive.udpString;  //  Update our score continuously.
        //udpString = ((UDPReceive)this.GetComponent("UDPReceive")).udpString;
        udpString = GetComponent<UDPReceive>().udpString;
        //Debug.Log(udpString);
        JSONCityMatrix data = JsonUtility.FromJson<JSONCityMatrix>(udpString);


        foreach (JSONBuilding b in data.grid)
        {
            b.Correct(15, 15);
            city[b.x, b.y].JSONUpdate(b);
        }
        //BuildingDataCtrl.instance.UpdateDensities(data.objects.density);

        this.oldData = data;

    }

    // Update is called once per frame
    void Update () {

        udpString = GetComponent<UDPReceive>().udpString;
        JSONCityMatrix data = JsonUtility.FromJson<JSONCityMatrix>(udpString);


        for (int i = 0; i < data.grid.Length; i++)
        {
            JSONBuilding a = data.grid[i];
            a.Correct(15, 15);
            city[a.x, a.y].JSONUpdate(a);
        }
        BuildingDataCtrl.instance.UpdateDensities(data.objects.density);
    }

    /*
    IEnumerator Initialize()
    {
        //yield return null;
        //WWW jsonPage = new WWW(this.JsonURL);
        //yield return jsonPage;
        //JSONCityMatrix data = JsonUtility.FromJson<JSONCityMatrix>(jsonPage.text);

        // read file, acturally reading and parsing works but the file cannot be open with 2 programs
        //readJson = sReader.ReadToEnd();
        //JSONCityMatrix data = JsonUtility.FromJson<JSONCityMatrix>(readJson); 

        // from UDP
        yield return null;
        //WWW jsonPage = new WWW(this.JsonURL);
        yield return udpString;
        udpString = UDPReceive.udpString;  //  Update our score continuously.
        Debug.Log(udpString);
        JSONCityMatrix data = JsonUtility.FromJson<JSONCityMatrix>(udpString);


        foreach (JSONBuilding b in data.grid)
        {
            b.Correct(15, 15);
            city[b.x, b.y].JSONUpdate(b);
        }
        //BuildingDataCtrl.instance.UpdateDensities(data.objects.density);

        this.oldData = data;
        StartCoroutine("CheckForUpdates");
    }

    IEnumerator CheckForUpdates()
    {
        //WWW jsonPage = new WWW(this.JsonURL);
        //yield return jsonPage;
        //while (jsonPage.error != null)
        //{
        //    Debug.Log(jsonPage.error);
        //    jsonPage = new WWW(this.JsonURL);
        //    float t = Time.time;
        //    yield return jsonPage;
        //}
        //Debug.Log(Time.time - t);



        //JSONCityMatrix data = JsonUtility.FromJson<JSONCityMatrix>(jsonPage.text);


        // from UDP
        yield return udpString;
        udpString = UDPReceive.udpString;  //  Update our score continuously.
        Debug.Log(udpString);
        JSONCityMatrix data = JsonUtility.FromJson<JSONCityMatrix>(udpString);


        for (int i = 0; i < data.grid.Length; i ++)
        {
            JSONBuilding a = data.grid[i];
            a.Correct(15,15);
            city[a.x, a.y].JSONUpdate(a);
        }
        BuildingDataCtrl.instance.UpdateDensities(data.objects.density);
        StartCoroutine("CheckForUpdates");
    }
    */
}

[Serializable]
class JSONCityMatrix
{
    public JSONBuilding[] grid;
    public JSONObjects objects;
    public int new_delta;
}

[Serializable]
public class JSONBuilding
{
    public int type;
    public int x;
    public int y;
    public int magnitude;
    public int rot;

    public override bool Equals(object obj)
    {
        JSONBuilding o = obj as JSONBuilding;
        return o != null &&
            this.type == o.type &&
            this.x == o.x &&
            this.y == o.y &&
            this.magnitude == o.magnitude &&
            this.rot == o.rot;
    }

    public override int GetHashCode()
    {
        return this.type.GetHashCode() *
            this.x.GetHashCode() *
            this.y.GetHashCode() *
            this.magnitude.GetHashCode() *
            this.rot.GetHashCode();
    }
    /// <summary>
    /// Normalizes this building into bottom-left origin coordinates
    /// </summary>
    public void Correct(int maxX, int maxY)
    {
        this.x = maxX - this.x;
        this.y = maxY - this.y;
    }

    /// <summary>
    /// Returns true if this new JSONBuildingData alters the input buildingData a
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public bool Changes(BuildingModel a)
    {
        return this.x == a.x &&
            this.y == a.y &&
            (this.type != a.Id ||
            this.magnitude != a.Magnitude ||
            this.rot != a.Rotation);
    }
}

[Serializable]
class JSONObjects
{
    public int pop_mid;
    public int toggle2;
    public int pop_old;
    public int[] density;
    public int IDMax;
    public double slider1;
    public int toggle1;
    public int dockRotation;
    public int pop_young;
    public int gridIndex;
    public int dockID;
    public int toggle3;
}