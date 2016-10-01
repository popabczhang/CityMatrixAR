using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BuildingDataCtrl : MonoBehaviour
{
    public static BuildingDataCtrl instance = null;

    public IdDataStruct[] buildingTypes;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
    }

    internal void UpdateBuildingModel(BuildingModel model, int id)
    {
        IdDataStruct type = System.Array.Find(buildingTypes, a => a.ID == id);
        if (type != null)
        {
            model.Height = type.height;
            model.Width = type.width;
        }
        
    }

    [System.Serializable]
    public class IdDataStruct
    {
        public int ID;
        public float height;
        public float width;
    }
}

public class BuildingModel {

    public VirtualCityModel parentModel;
    private int _id;
    public int Id {
        get {
            return _id; }
        set
        {
            BuildingDataCtrl.instance.UpdateBuildingModel(this, value);
            _id = value; }
    }
    public int x;
    public int y;
    private float _height;
    public float Height {
        get { return _height; }
        set { _height = value; }
    }
    private float _width;
    public float Width {
        get { return _width; }
        set { _width = value; }
    }
    private int _rotation;
    public int Rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
    }
    private int _magnitude;
    public int Magnitude
    {
        get { return _magnitude; }
        set { _magnitude = value; }
    }
    private double[,] _heatMap;
    public double[,] HeatMap
    {
        get { return _heatMap; }
        set { _heatMap = value; }
    }
    private double _colorRef;
    public double ColorRef
    {
        get { return _colorRef; }
        set { _colorRef = value; }
    }
    private Building[] _views;
    public Building[] Views
    {
        get { return _views; }
        set { _views = value; }
    }

    public BuildingModel()
    {
        this._id = -1;
        this.x = 0;
        this.y = 0;
        this._rotation = 0;

        this._height = 0.1f;
        this._width = 30;
        this._magnitude = 0;
        this._heatMap = new double[7, 7];
    }

    public void JSONUpdate(JSONBuilding b)
    {
        this.Id = b.type;
        this.Rotation = b.rot;
        this.Magnitude = b.magnitude;
    }
    
    public BuildingModel Copy()
    {
        BuildingModel a = (BuildingModel)this.MemberwiseClone();
        a._heatMap = new double[7, 7];
        return a;
    }
}
