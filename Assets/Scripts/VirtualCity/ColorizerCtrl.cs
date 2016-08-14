using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class ColorizerCtrl : MonoBehaviour {

    public VirtualCityModel cityModel;
    public string JsonURL;

    private JSONCityMatrix oldData;

	// Use this for initialization
	void Start () {
        StartCoroutine("Initialize");
    }

    // Update is called once per frame
    void Update () {
	}

    IEnumerator Initialize()
    {
        yield return null;
        WWW jsonPage = new WWW(this.JsonURL);
        yield return jsonPage;
        JSONCityMatrix data = JsonUtility.FromJson<JSONCityMatrix>(jsonPage.text);

        foreach(JSONBuilding b in data.grid)
        {
            this.cityModel.updateBuilding(b.type, b.x, b.y, b.rot);
        }

        this.oldData = data;
        StartCoroutine("CheckForUpdates");
    }

    IEnumerator CheckForUpdates()
    {
        WWW jsonPage = new WWW(this.JsonURL);
        yield return jsonPage;
        JSONCityMatrix data = JsonUtility.FromJson<JSONCityMatrix>(jsonPage.text);

        for(int i = 0; i < data.grid.Length; i ++)
        {
            if(!data.grid[i].Equals(oldData.grid[i]))
            {
                JSONBuilding b = data.grid[i];
                this.cityModel.updateBuilding(b.type, b.x, b.y, b.rot);
            }
        }
        StartCoroutine("CheckForUpdates");
    }
}

[Serializable]
class JSONCityMatrix
{
    public JSONBuilding[] grid;
    public JSONObjects objects;
    public int new_delta;
}

[Serializable]
class JSONBuilding
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