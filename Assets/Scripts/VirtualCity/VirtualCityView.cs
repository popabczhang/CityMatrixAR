using UnityEngine;
using System.Collections;

public class VirtualCityView : MonoBehaviour {

    public GameObject virtualCityModelObj;
    private VirtualCityModel virtualCityModel;
    public Building.Type _viewType;
    public Building.Type ViewType {
        get { return _viewType; }
        set {
            foreach (Building b in this.city) {
                b.ViewType = value;
            }
            _viewType = value;
        }
    }
    public int Width;
    public int Length;

    public GameObject buildingPrefab;

    private float _spacing;
    public float Spacing
    {
        get { return _spacing; }
        set {
            _spacing = value;
            this.RepositionAll();
        }
    }

    private Building[,] city;

	// Use this for initialization
	void Start () {
        virtualCityModel = virtualCityModelObj.GetComponent<VirtualCityModel>();
        city = new Building[this.Width, this.Length];
        for(int i = 0; i < this.Width; i ++)
        {
            for(int j = 0; j < this.Length; j ++) {
                Transform bObj = Instantiate(this.buildingPrefab).transform;
                bObj.parent = this.transform;
                bObj.localPosition = this.GetBuildingPos(i, j);
                Building b = bObj.GetComponent<Building>();
                b.virtualCityView = this;
                
                city[i, j] = b;
                virtualCityModel.InitializeView(i, j, b);
                b.ViewType = this.ViewType;
            } 
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Vector3 GetBuildingPos(int i, int j)
    {
        return new Vector3((1 + this.Spacing) * i, 0, (1 + this.Spacing) * j);
    }

    void RepositionAll()
    {
        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Length; j++)
            {
                Transform bObj = this.city[i,j].transform;
                bObj.localPosition = this.GetBuildingPos(i, j);
            }
        }
    }
}
