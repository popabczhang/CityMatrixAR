using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BuildingDataCtrl : MonoBehaviour
{
    public static BuildingDataCtrl instance = null;

    public GameObject[] residentialLowRise;
    public GameObject[] residentialMidRise;
    public GameObject[] residentialHighRise;
    public GameObject[] officeLowRise;
    public GameObject[] officeMidRise;
    public GameObject[] officeHighRise;
    public GameObject[] road;
    public GameObject[] park;
    public GameObject[] flat;

    public IdDataStruct[] buildingTypes;

    internal List<int> density = new List<int>(new int[] { 1, 1, 1, 1, 1, 1 });

    internal List<BuildingModel> models = new List<BuildingModel>();

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
            float r = Random.Range(0f, 1f);
            model.FlatView = id == -1 ? (r > 0.25f ? park[Random.Range(0, park.Length - 1)] :
                    flat[Random.Range(0, flat.Length - 1)]) :
                    type.flatView;
            if (id == -1)
            {
                model.MeshView = r > 0.25f ? park[Random.Range(0, park.Length - 1)] : 
                    flat[Random.Range(0, flat.Length - 1)];
            } else if(id == 6)
            {
                model.MeshView = road[Random.Range(0, road.Length - 1)];
            } else if(density[id] <= 6)
            {
                model.MeshView = type.residential ? 
                    residentialLowRise[Random.Range(0, residentialLowRise.Length - 1)] :
                    officeLowRise[Random.Range(0, officeLowRise.Length - 1)];
            } else if (density[id] <= 15)
            {
                model.MeshView = type.residential ?
                    residentialMidRise[Random.Range(0, residentialMidRise.Length - 1)] :
                    officeMidRise[Random.Range(0, officeMidRise.Length - 1)];
            } else
            {
                model.MeshView = type.residential ?
                    residentialHighRise[Random.Range(0, residentialHighRise.Length - 1)] :
                    officeHighRise[Random.Range(0, officeHighRise.Length - 1)];
            }
        }
    }

    internal void UpdateDensities(int[] densities) {
        this.density = new List<int>(); 
        for(int i = 0; i < densities.Length; i ++ )
        {
            this.density.Add(densities[i]);
        }
    }

    [System.Serializable]
    public class IdDataStruct
    {
        public int ID;
        public float height;
        public float width;
        public GameObject flatView;
        public bool residential;
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

    private GameObject _flatView;
    public GameObject FlatView
    {
        get { return _flatView; }
        set { _flatView = value;
            foreach(Building b in views)
            {
                b.flatPrefab = value;
            }
        }
    }
    private GameObject _meshView;
    public GameObject MeshView
    {
        get { return _meshView; }
        set
        {
            _meshView = value;
            foreach (Building b in views)
            {
                b.meshPrefab = value;
            }
        }
    }


    private List<Building> views = new List<Building>();

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
        BuildingDataCtrl.instance.models.Add(this);
        BuildingDataCtrl.instance.UpdateBuildingModel(this, -1);
    }

    public void AddView(Building b)
    {
        views.Add(b);
        b.Height = this.GetVirtualHeight();
        b.flatPrefab = this.FlatView;
        b.meshPrefab = this.MeshView;
        //b.Rotation = this.Rotation;

    }

    public float GetVirtualHeight()
    {
        return this.Height / this.Width;
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
