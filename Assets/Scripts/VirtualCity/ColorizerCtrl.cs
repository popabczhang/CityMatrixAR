using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class ColorizerCtrl : MonoBehaviour {

    public TextAsset testData;

    public VirtualCityModel cityModel;

    private StreamReader stream;
    private Boolean initialized = false;

	// Use this for initialization
	void Start () {
        this.stream = new StreamReader(
            new MemoryStream(
                Encoding.UTF8.GetBytes(this.testData.text ?? "")));

        StartCoroutine("Initialize");
    }
	
	// Update is called once per frame
	void Update () {
        this.checkForUpdate();
	}

    IEnumerator Initialize()
    {
        this.parseData(this.stream);
        yield return null;
    }

    void parseLine(string line)
    {
        string[] data = line.Split('\t');
        cityModel.editBuilding(
            Int32.Parse(data[0]), Int32.Parse(data[1]), 
            Int32.Parse(data[2]), Int32.Parse(data[3]));
    }

    void parseData(StreamReader data)
    {
        while(!data.EndOfStream)
        {
            parseLine(data.ReadLine());
        }
    }

    void checkForUpdate()
    {
        
    }
}
